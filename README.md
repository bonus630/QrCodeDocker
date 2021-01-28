# QrCodeDocker
CorelDraw Addon X7 or Higher, provides a creation of qr codes in offline mode, in batch and customization, uses the zxing library to codify and decodify the qrcode. Dont require internet or autentication. Please see link below to features demonstration

## How add a new language
Add a new class in "QrCodeDocker/Lang", this file need be named according of your language, use this table like reference 
https://docs.microsoft.com/pt-br/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c
and finally implements the "ILang" interface, the language will be loaded in coreldraw according coreldraw language
