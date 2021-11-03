
#include <ESP8266WiFi.h>
#include <WiFiUDP.h>
#include <settings.h>
#include <WiFiClient.h>
#include <ESP8266WebServer.h>
#include <ESP8266mDNS.h>
#include <ESP8266HTTPUpdateServer.h>

ESP8266WebServer httpServer(80);
ESP8266HTTPUpdateServer httpUpdater;
WiFiUDP Udp;

#define pinR 12
#define pinG 13
#define pinB 14
#define pinW 5

#define Dn_2k2 4
#define Dn_10k 2
#define pinButtonRes 5
const char *host = "esp8266-webupdate";
//----------------
byte name = 0;
byte group = 0;
// ----------------
char incomingPacket[255];
char replyPacket[] = "";
bool AP = false;
// настройки пламени
byte HUE_START = 140;  // начальный цвет огня (0 красный, 80 зелёный, 140 молния, 190 розовый)
byte HUE_GAP = 10;     // коэффициент цвета огня (чем больше - тем дальше заброс по цвету)
float SMOOTH_K = 0.17; // коэффициент плавности огня
byte MIN_BRIGHT = 80;  // мин. яркость огня
byte MAX_BRIGHT = 255; // макс. яркость огня
byte MIN_SAT = 255;    // мин. насыщенность
byte MAX_SAT = 255;    // макс. насыщенность
// ----------------
float brightness = 255;
byte numEffect = 0;
byte valueW = 0;
long int speedEffectTimer = 0;
int speedEffect = 8;
//----------
byte stepColor = 1;
int numColor = 1;
bool flagRender = true;
bool flagRain = true;
//----------
boolean strobFlag = true;
byte saveBrigh;
//----------
int16_t temperature = 0;
//----------
typedef struct
{
  int r; // a fraction between 0 and 1
  int g; // a fraction between 0 and 1
  int b; // a fraction between 0 and 1
} RgbColor;

typedef struct
{
  int h; // angle in degrees
  int s; // a fraction between 0 and 1
  int v; // a fraction between 0 and 1
} HsvColor;
HsvColor dishsv{100, 100, 255};
RgbColor dis{0, 120, 30};
int countTime = 0;

RgbColor HsvToRgb(HsvColor hsv)
{
  RgbColor rgb;
  unsigned char region, remainder, p, q, t;

  if (hsv.s == 0)
  {
    rgb.r = hsv.v;
    rgb.g = hsv.v;
    rgb.b = hsv.v;
    return rgb;
  }

  region = hsv.h / 43;
  remainder = (hsv.h - (region * 43)) * 6;

  p = (hsv.v * (255 - hsv.s)) >> 8;
  q = (hsv.v * (255 - ((hsv.s * remainder) >> 8))) >> 8;
  t = (hsv.v * (255 - ((hsv.s * (255 - remainder)) >> 8))) >> 8;

  switch (region)
  {
  case 0:
    rgb.r = hsv.v;
    rgb.g = t;
    rgb.b = p;
    break;
  case 1:
    rgb.r = q;
    rgb.g = hsv.v;
    rgb.b = p;
    break;
  case 2:
    rgb.r = p;
    rgb.g = hsv.v;
    rgb.b = t;
    break;
  case 3:
    rgb.r = p;
    rgb.g = q;
    rgb.b = hsv.v;
    break;
  case 4:
    rgb.r = t;
    rgb.g = p;
    rgb.b = hsv.v;
    break;
  default:
    rgb.r = hsv.v;
    rgb.g = p;
    rgb.b = q;
    break;
  }

  return rgb;
}
void HSVtoRGBW()
{
  valueW = 255 - dishsv.s;
  dishsv.s = 255;
}
void rainbow_hsv()
{
  dishsv.s = 125;
  dishsv.v = 255;
  if (flagRain)
  {
    dishsv.h -= stepColor;
    if (dishsv.h - stepColor <= 0)
    {
      flagRain = false;
    }
  }
  else
  {
    dishsv.h += stepColor;
    if (dishsv.h + stepColor >= 255)
    {
      flagRain = true;
    }
  }
  dis = HsvToRgb(dishsv);
}
void setKelvin()
{
  float tmpKelvin, tmpCalc;

  temperature = constrain(temperature, 1000, 40000);
  tmpKelvin = temperature / 100;

  // red
  if (tmpKelvin <= 66)
    dis.r = 255;
  else
  {
    tmpCalc = tmpKelvin - 60;
    tmpCalc = (float)pow(tmpCalc, -0.1332047592);
    tmpCalc *= (float)329.698727446;
    tmpCalc = constrain(tmpCalc, 0, 255);
    dis.r = tmpCalc;
  }

  // green
  if (tmpKelvin <= 66)
  {
    tmpCalc = tmpKelvin;
    tmpCalc = (float)99.4708025861 * log(tmpCalc) - 161.1195681661;
    tmpCalc = constrain(tmpCalc, 0, 255);
    dis.g = tmpCalc;
  }
  else
  {
    tmpCalc = tmpKelvin - 60;
    tmpCalc = (float)pow(tmpCalc, -0.0755148492);
    tmpCalc *= (float)288.1221695283;
    tmpCalc = constrain(tmpCalc, 0, 255);
    dis.g = tmpCalc;
  }

  // blue
  if (tmpKelvin >= 66)
    dis.b = 255;
  else if (tmpKelvin <= 19)
    dis.b = 0;
  else
  {
    tmpCalc = tmpKelvin - 10;
    tmpCalc = (float)138.5177312231 * log(tmpCalc) - 305.0447927307;
    tmpCalc = constrain(tmpCalc, 0, 255);
    dis.b = tmpCalc;
  }
}

void rainbow2_hsv()
{
  dishsv.s = 255;
  dishsv.v = 255;
  if (flagRain)
  {
    dishsv.h += stepColor;
  }
  else
  {
    dishsv.h -= stepColor;
  }
  if (dishsv.h < 0)
  {
    dishsv.h = 0;
    flagRain = true;
  }
  else if (dishsv.h > 255)
  {
    dishsv.h = 255;
    flagRain = false;
  }
  dis = HsvToRgb(dishsv);
}
void setColor(int color)
{
  if (color <= 255)
  { // красный макс, зелёный растёт
    dis.r = 255;
    dis.g = color;
    dis.b = 0;
  }
  else if (color > 255 && color <= 510)
  { // зелёный макс, падает красный
    dis.r = 510 - color;
    dis.g = 255;
    dis.b = 0;
  }
  else if (color > 510 && color <= 765)
  { // зелёный макс, растёт синий
    dis.r = 0;
    dis.g = 255;
    dis.b = color - 510;
  }
  else if (color > 765 && color <= 1020)
  { // синий макс, падает зелёный
    dis.r = 0;
    dis.g = 1020 - color;
    dis.b = 255;
  }
  else if (color > 1020 && color <= 1275)
  { // синий макс, растёт красный
    dis.r = color - 1020;
    dis.g = 0;
    dis.b = 255;
  }
  else if (color > 1275 && color <= 1530)
  { // красный макс, падает синий
    dis.r = 255;
    dis.g = 0;
    dis.b = 1530 - color;
  }
}
void rainbowColor()
{

  if (flagRain)
  {
    numColor += stepColor;
  }
  else
  {
    numColor -= stepColor;
  }
  if (numColor < 0)
  {
    numColor = 0;
    flagRain = true;
  }
  else if (numColor > 1530)
  {
    numColor = 1530;
    flagRain = false;
  }

  setColor(numColor);
}
void fireTick()
{
  static uint32_t prevTime, prevTime2;
  static byte fireRnd = 0;
  static float fireValue = 0;

  // задаём направление движения огня
  if (millis() - prevTime > 100)
  {
    prevTime = millis();
    fireRnd = random(0, 10);
  }
  // двигаем пламя
  if (millis() - prevTime2 > 20)
  {
    prevTime2 = millis();
    fireValue = (float)fireValue * (1 - SMOOTH_K) + (float)fireRnd * 10 * SMOOTH_K;
    dishsv.h = constrain(map(fireValue, 20, 60, HUE_START, HUE_START + HUE_GAP), 0, 255);
    dishsv.s = constrain(map(fireValue, 20, 60, MAX_SAT, MIN_SAT), 0, 255);
    dishsv.v = constrain(map(fireValue, 20, 60, MIN_BRIGHT, MAX_BRIGHT), 0, 255);
    dis = HsvToRgb(dishsv);
  }
}
void parser()
{
  int numOrder[10] = {0, 0, 0, 0, 0, 0, 0, 0, 0};
  int count = 0;
  byte comName;
  byte comGroup;
  char *pointer = incomingPacket;
  /*
  byte comName = atoi(incomingPacket);
  pointer = incomingPacket + 4;
  byte comGroup = atoi(pointer);
  pointer = incomingPacket + 8;
  Serial.println("-----------");
  Serial.println(pointer);
  */
  while (true)
  {

    numOrder[count++] = atoi(pointer);
    pointer = strchr(pointer, ':');

    if (pointer)
      pointer++;
    else
      break;
  }
  comName = numOrder[0];
  comGroup = numOrder[1];
  Serial.println(comGroup);
  Serial.println(comName);
  if (comName == name || comGroup == group || comName == 0)
  {

    Serial.println("-----------");
    Serial.println(numOrder[0]);
    Serial.println(numOrder[1]);
    Serial.println(numOrder[2]);
    Serial.println(numOrder[3]);
    Serial.println(numOrder[4]);
    Serial.println(numOrder[5]);
    Serial.println(numOrder[6]);
    Serial.println("-----------");

    if (numOrder[2] == 0)
    {
      char *data;
      switch (numOrder[3])
      {
      case 0:
        brightness = numOrder[4];
        break;
      case 1:
        speedEffect = numOrder[4];
        break;
      case 2:
        settings.name = numOrder[4];
        settings_save();
        name = numOrder[4];
        break;
      case 3:
        settings.group = numOrder[4];
        settings_save();
        group = numOrder[4];
        break;
      case 4:
        data = strrchr(incomingPacket + 8, ':');
        settings.pass = data + 1;
        *data = 0;
        data = strrchr(incomingPacket + 8, ':');
        settings.ssid = data + 1;
        settings.AP = false;
        settings_save();
        break;
      case 5:
        data = strrchr(incomingPacket + 8, ':');
        settings.NameAP = data + 1;
        settings.AP = true;
        settings_save();
        break;
      case 6:
        Udp.beginPacket(Udp.remoteIP(), numOrder[4]);
        byte byf[2] = {name, group};
        Udp.write(byf, sizeof(byf));
        Serial.println("Отправка");
        Udp.endPacket();
        break;
      }
    }
    else
    {
      switch (numOrder[3])
      {
      case 0:
        numEffect = 0;
        brightness = 255;

        dis.r = numOrder[4];
        dis.g = numOrder[5];
        dis.b = numOrder[6];
        break;
      case 1:
        numEffect = 0;
        temperature = numOrder[4];
        setKelvin();
        break;
      case 2:
        numEffect = 0;
        numColor = numOrder[4];
        rainbowColor();
        break;
      case 3:
        speedEffect = numOrder[5];
        numEffect = numOrder[4];
        break;
      case 4:
        speedEffect = numOrder[5];
        HUE_START = numOrder[4];
        if (numOrder[6] != 0)
        {
          SMOOTH_K = (float)numOrder[6] / 100;
        }
        numEffect = 5;
        break;
      case 5:
        speedEffect = numOrder[4];
        Serial.println(speedEffect);
        numEffect = 6;
        break;
      case 6:
        valueW = numOrder[4];
        break;
      }
    }
    flagRender = true;
  }
}
void rendering()
{

  if (numEffect > 0)
  {
    flagRender = true;
    speedEffectTimer = millis();
    switch (numEffect)
    {

    case 2:
      rainbowColor();
      break;
    case 3:
      rainbow_hsv();
      break;
    case 4:
      rainbow2_hsv();
      break;
    case 5:
      fireTick();

      break;
    case 6:
      if (strobFlag)
      {
        saveBrigh = brightness;
        brightness = 0;
        strobFlag = false;
      }
      else
      {
        brightness = saveBrigh;
        strobFlag = true;
      }
      break;
    }
  }
  if (flagRender)
  {
    /*
    Serial.println(dis.r);
    Serial.println(dis.g);
    Serial.println(dis.b);
    Serial.println(dis.r * (brightness / 255));
    Serial.println(dis.g * (brightness / 255));
    Serial.println(dis.b * (brightness / 255));
    */
    analogWrite(pinR, dis.r * (brightness / 255));
    analogWrite(pinG, dis.g * (brightness / 255));
    analogWrite(pinB, dis.b * (brightness / 255));
    analogWrite(pinW, valueW * (brightness / 255));
    flagRender = false;
  }
}
void setup()
{
  pinMode(pinR, OUTPUT);
  pinMode(pinB, OUTPUT);
  pinMode(pinG, OUTPUT);
  pinMode(pinW, OUTPUT);
  pinMode(pinButtonRes, INPUT_PULLUP);
  Serial.begin(115200);
  analogWriteRange(255);
  analogWriteFreq(10000);
  pinMode(Dn_2k2, OUTPUT);
  pinMode(Dn_10k, OUTPUT);
  digitalWrite(Dn_2k2, LOW);
  digitalWrite(Dn_10k, LOW);
  delay(3000);

  pinMode(Dn_2k2, OUTPUT);
  pinMode(Dn_10k, OUTPUT);
  digitalWrite(Dn_2k2, LOW);
  digitalWrite(Dn_10k, HIGH);
  if (!digitalRead(pinButtonRes))
  {
    settings_save();
    Serial.println("сброс");
  } 
  settings_read();

  name = settings.name;
  group = settings.group;
  AP = settings.AP;
  if (AP)
  {
    WiFi.mode(WIFI_AP);
    Serial.println(WiFi.softAP(settings.NameAP) ? "Ready" : "Failed!");
    dis = {120, 0, 30};
  }
  else
  {
    WiFi.begin(settings.ssid, settings.pass);
    int count = 0;
    WiFi.mode(WIFI_STA);
    Serial.print("Connecting"); //  "Подключение"
    Serial.println(WiFi.softAPdisconnect(false) ? "Ready" : "Failed!");
    while (WiFi.status() != WL_CONNECTED)
    {
      if (count <= settings.time_wifi * 1000 / 500)
      {
        delay(250);
        analogWrite(pinR, 0);
        analogWrite(pinG, 50);
        analogWrite(pinB, 200);
        delay(250);
        analogWrite(pinR, 50);
        analogWrite(pinG, 0);
        analogWrite(pinB, 50);
        Serial.print(".");
        count++;
      }
      else
      {
        WiFi.mode(WIFI_AP);
        Serial.println(WiFi.softAP(settings.NameAP) ? "Ready" : "Failed!");
        dis = {120, 0, 30};
        break;
      }
    }
    
  }
  Serial.println();

  MDNS.begin(host);

  httpUpdater.setup(&httpServer);
  httpServer.begin();

  MDNS.addService("http", "tcp", 80);
  Serial.printf("HTTPUpdateServer ready! Open http://%s.local/update in your browser\n", host);
  Serial.print("Connected, IP address: ");
  //  "Подключились, IP-адрес: "
  Serial.println(WiFi.localIP());
  Udp.beginMulticast(WiFi.localIP(), IPAddress(226, 1, 1, 1), 4096);
  Serial.println("\n connected udp multicast");
}

void loop()
{
  httpServer.handleClient();
  MDNS.update();
  int packetSize = Udp.parsePacket();
  if (packetSize)
  {
    // receive incoming UDP packets
    Serial.printf("Received %d bytes from %s, port %d\n", packetSize, Udp.remoteIP().toString().c_str(), Udp.remotePort());
    int len = Udp.read(incomingPacket, 255);
    if (len > 0)
    {
      incomingPacket[len] = '\0';
    }
    Serial.println("UDP packet contents: %s\n");
    Serial.println(incomingPacket);
    Udp.endPacket();
    parser();
  }
  if (millis() - countTime >= speedEffect + 8)
  {
    countTime = millis();
    rendering();
  }
  if (Serial.available() > 0)
  {
    numEffect = Serial.readString().toInt();
    Serial.println("Эффект номер");
    Serial.println(numEffect);
    delay(1000);
  }
}
