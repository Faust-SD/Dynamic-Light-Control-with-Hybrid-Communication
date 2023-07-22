#include <ESP8266WiFi.h>
#include <WiFiUdp.h>

class KalmanFilter {
public:
    KalmanFilter(double process_noise, double measurement_noise, double estimation_error, double initial_value)
    : q(process_noise), r(measurement_noise), p(estimation_error), x(initial_value) {}

    void update(double measurement) {
        p += q;
        k = p / (p + r);
        x = x + k * (measurement - x);
        p = (1 - k) * p;
    }

    double value() { return x; }

private:
    double q; // process noise covariance
    double r; // measurement noise covariance
    double x; // value
    double p; // estimation error covariance
    double k; // kalman gain
};

const char* ssid = "eight";
const char* password = "00000001";

WiFiUDP Udp;
IPAddress remoteIP(192, 168, 16, 169); // 这是你的Unity机器的IP地址
unsigned int remotePort = 8000; // 这是你的Unity应用程序正在监听的端口

void setup() {
  Serial.begin(115200);

  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting...");
  }

  Serial.println("Connected!");
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());
}

void loop() {
  static KalmanFilter filter(0.1, 1, 1, 0); // Initialize the Kalman filter with arbitrary values.
  int photoResistorValue = analogRead(A0); // 假设你的光敏电阻连接到A0
   filter.update(photoResistorValue);
   int out = int(filter.value());
  String message = String(out);
  
  Udp.beginPacket(remoteIP, remotePort);
  Udp.write(message.c_str());
  Udp.endPacket();
  Serial.println(message.c_str());
  delay(10); // 每秒发送一次
}
