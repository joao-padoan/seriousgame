#include <Filters.h>

#include <AH/Timing/MillisMicrosTimer.hpp>
#include <Filters/Butterworth.hpp>
#include <math.h>

#include <WiFi.h>
#include <Firebase_ESP_Client.h>

#include "addons/TokenHelper.h"
#include "addons/RTDBHelper.h"

#define LED_BUILTIN 2

#define WIFI_SSID "HUAWEI-CV1V5Y4D"
#define WIFI_PASSWORD "20051990"

// Insert Firebase project API Key
#define API_KEY "AIzaSyB2Las9aYlzVp8FlD_lC94IoHoyXv5wWY4"

// Insert RTDB URLefine the RTDB URL 
#define DATABASE_URL "https://teste-f9f61-default-rtdb.firebaseio.com/"

//Define Firebase Data object
FirebaseData fbdo;

FirebaseAuth auth;
FirebaseConfig config;

unsigned long sendDataPrevMillis = 0;
int count = 0;
bool signupOK = false;

void setup() {
  pinMode(LED_BUILTIN, OUTPUT);
  Serial.begin(115200);

  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
  Serial.print("Connecting to Wi-Fi");
  while (WiFi.status() != WL_CONNECTED) {
    Serial.print(".");
    delay(300);
  }
  Serial.println();
  Serial.print("Connected with IP: ");
  Serial.println(WiFi.localIP());
  Serial.println();

  config.api_key = API_KEY;

  config.database_url = DATABASE_URL;

  if (Firebase.signUp(&config, &auth, "", "")) {
    Serial.println("ok");
    signupOK = true;
  } else {
    Serial.printf("%s\n", config.signer.signupError.message.c_str());
  }

  config.token_status_callback = tokenStatusCallback;  //see addons/TokenHelper.h

  Firebase.begin(&config, &auth);
  Firebase.reconnectWiFi(true);
}

int sinal;
// Frequência de amostragem
const double f_s = 100;  // Hz
// Frequência de corte
const double f_c = 350;  // Hz
// Frequência de corte normalizada
const double f_n = 2 * f_c / f_s;

// Timer de amostra
Timer<micros> timer = std::round(1e6 / f_s);

// Filtro Butterworth de 4ª ordem
auto filter = butter<4>(f_n);

// Variáveis para cálculo do RMS
const unsigned long windowDuration = 30;  // 30 milissegundos
unsigned long lastTime = 0;
double sumSquares = 0.0;
unsigned long sampleCount = 0;

void loop() {
  sinal = map(analogRead(A0), 0, 4095, -3.3, 3.3);

  if (timer) {
    Serial.print(3.3);
    Serial.print(", ");
    Serial.print(-3.3);
    Serial.print(", ");
    // Aplica o filtro
    double filteredSinal = filter(sinal);

    // Atualiza o tempo
    unsigned long currentTime = millis();

    // Acumula os quadrados do sinal
    sumSquares += filteredSinal * filteredSinal;
    sampleCount++;

    // Se 50ms passaram, calcula o RMS
    if (currentTime - lastTime >= windowDuration) {
      // Verifica se temos amostras
      if (sampleCount > 0) {
        double rms = sqrt(sumSquares / sampleCount);
        rms = abs(rms);

        if (Firebase.RTDB.setFloat(&fbdo, "test/float", rms)) {
          Serial.println("PASSED");
          Serial.println("PATH: " + fbdo.dataPath());
          Serial.println("TYPE: " + fbdo.dataType());
        } else {
          Serial.println("FAILED");
          Serial.println("REASON: " + fbdo.errorReason());
        }
      }

      // Reseta as variáveis
      lastTime = currentTime;
      sumSquares = 0.0;
      sampleCount = 0;
    } else {
      Serial.print(0);
      Serial.print(", ");
    }

    // Exibe o sinal filtrado
    Serial.println(filteredSinal);
  }
}
