
#include "FS.h"

const char *settingsFilename = "/settings.cfg";

struct settings_t
{
  bool AP;
  String ssid;
  String pass;
  String NameAP;
  byte name;
  byte group;
  int time_wifi;
};

extern settings_t settings{//default
                           false,
                           "RT-WiFi_91D8",
                           "JVMJ2C65",
                           "SettingsAP",
                           100,
                           100,
                           60};

bool settings_save()
{
  if (!SPIFFS.begin())
  {
#ifdef DEBUG
    Serial.println("SPIFFS mount failed");
#endif
    return false;
  }
  File config = SPIFFS.open(settingsFilename, "w");
  if (!config)
  {
#ifdef DEBUG
    Serial.println("File open failed");
#endif
    return false;
  }
  uint16_t bytesWrited = config.write((byte *)&settings, sizeof(settings));
  config.close();
  if (bytesWrited != sizeof(settings))
  {
#ifdef DEBUG
    Serial.println("writen != settings");
#endif
    return false;
  }
  else
  {
    return true;
  }
}

bool settings_read()
{
  if (!SPIFFS.begin())
  {
#ifdef DEBUG
    Serial.println("SPIFFS mount failed");
#endif
    return false;
  }
  if (!SPIFFS.exists(settingsFilename))
  {
#ifdef DEBUG
    Serial.println("Config file not exsist");
#endif
    if (!settings_save())
    {
      return false;
    }
  }
  File config = SPIFFS.open(settingsFilename, "r");
  if (!config)
  {
    return false;
  }
  uint16_t bytesRead = config.read((byte *)&settings, sizeof(settings));
  config.close();
  if (bytesRead != sizeof(settings))
  {
    return false;
  }
  else
  {
    return true;
  }
}