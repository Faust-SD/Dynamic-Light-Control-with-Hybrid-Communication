

# 混合通信动态光照控制

混合通信动态光照控制是一个开源项目，利用物理设备（例如光敏电阻）检测实际环境光线，并根据接收到的数据动态地调整Unity环境中的光照条件。

## 项目特性

1. 使用Arduino和ESP8266WiFi开发板，通过光敏电阻读取环境光线。
2. 通过UDP和串行通信将环境光线数据传送至Unity应用程序。
3. Unity应用程序接收光线数据并动态调整场景光照。
4. 利用卡尔曼滤波进行数据平滑处理，更加逼真地模拟真实环境光照变化。
5. 包括太阳和月亮两个光源，模拟日夜交替。

## 如何使用

克隆或下载本项目，然后按照以下步骤进行操作：

1. 将Arduino代码上传到ESP8266WiFi开发板。
2. 在Unity中打开项目并运行。
3. 调整场景和物理设备以观察光照变化。

## 贡献

如果您发现错误或有改进的建议，请创建一个问题或提交一个拉取请求。


# Dynamic Light Control with Hybrid Communication

Dynamic Light Control with Hybrid Communication is an open source project that reads ambient light from a physical device, such as a photoresistor, and dynamically adjusts lighting conditions in a Unity environment based on the received data.

## Features

1. Reads ambient light with an Arduino and ESP8266WiFi board using a photoresistor.
2. Transmits the light data to a Unity application via UDP and Serial communication.
3. The Unity application receives the light data and dynamically adjusts the scene lighting.
4. Uses Kalman filtering for smooth data handling, simulating realistic ambient light changes more convincingly.
5. Includes two light sources, the sun and the moon, simulating the transition between day and night.

## How to Use

Clone or download this project and then follow these steps:

1. Upload the Arduino code to the ESP8266WiFi board.
2. Open the project in Unity and run.
3. Adjust the scene and physical device to observe the lighting changes.

## Contribution

If you find a bug or have a suggestion for improvement, please create an issue or submit a pull request.