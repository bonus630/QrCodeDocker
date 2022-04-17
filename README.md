# QrCodeDocker
CorelDraw Addon X7 or Higher, provides a creation of qr codes in offline mode, in batch and customization, uses the zxing library to codify and decodify the qrcode. Dont require internet or autentication. Please see link below to features demonstration

## How add a new language
Add a new class in "QrCodeDocker/Lang", use you language tag to name this file, use this table like reference 
https://docs.microsoft.com/pt-br/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c
to standarlize class names, replace - dash for _ underline and implements the "ILang" interface, the language will be loaded in coreldraw according coreldraw language, for extras repeate this steps. And finally add you language tag in LangTagsEnum and add a node in switch makes reference to cdrLanguage and your language https://github.com/bonus630/QrCodeDocker/blob/master/PluginLoader/LangTagsEnum.cs
