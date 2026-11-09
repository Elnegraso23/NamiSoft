; Instalador de NamiSoft - Inno Setup 6
; Compilar en Windows:  iscc /DMyAppVersion=1.0.0 NamiSoft.iss
; Requiere haber ejecutado antes:  dotnet publish -c Release -r win-x64 --self-contained true -o ..\publish\win-x64

#ifndef MyAppVersion
  #define MyAppVersion "1.0.0"
#endif

#define MyAppName "NamiSoft"
#define MyAppPublisher "NamiSoft"
#define MyAppURL "https://github.com/Elnegraso23/NamiSoft"
#define MyAppExeName "NamiSoft.exe"

[Setup]
AppId={{7C4B2A16-9E3D-4F58-9C1A-1B6D2E3F4A55}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}/releases
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=..\publish\instalador
OutputBaseFilename=NamiSoft-Setup-{#MyAppVersion}-x64
SetupIconFile=NamiSoft.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
UninstallDisplayName={#MyAppName} {#MyAppVersion}
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=admin
ShowLanguageDialog=no
AllowNoIcons=yes
CloseApplications=no
DisableWelcomePage=no
MinVersion=10.0

[Languages]
Name: "es"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "Crear un acceso directo en el escritorio"; GroupDescription: "Accesos directos:"
Name: "configbdd"; Description: "Configurar la conexion a SQL Server al terminar"; GroupDescription: "Base de datos:"

[Files]
Source: "..\publish\win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "Configurar-BD.cmd"; DestDir: "{app}"; Flags: ignoreversion
Source: "Configurar-BD.ps1"; DestDir: "{app}"; Flags: ignoreversion
Source: "NamiSoft.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\README.md"; DestDir: "{app}"; DestName: "LEEME.md"; Flags: ignoreversion
Source: "..\sql\NamiSoft_SQLServer.sql"; DestDir: "{app}\sql"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\NamiSoft.ico"
Name: "{group}\Configurar base de datos"; Filename: "{app}\Configurar-BD.cmd"; IconFilename: "{app}\NamiSoft.ico"; Comment: "Apunta la aplicacion a tu servidor SQL Server"
Name: "{group}\Carpeta de instalacion"; Filename: "{app}"
Name: "{group}\Desinstalar {#MyAppName}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\NamiSoft.ico"; Tasks: desktopicon

[Run]
Filename: "{app}\Configurar-BD.cmd"; Description: "Configurar la conexion a SQL Server"; Tasks: configbdd; Flags: runasoriginaluser postinstall skipifsilent
Filename: "{app}\{#MyAppExeName}"; Description: "Abrir {#MyAppName}"; Flags: nowait postinstall runasoriginaluser skipifsilent unchecked

[UninstallDelete]
Type: filesandordirs; Name: "{app}\logs"

[Messages]
es.WelcomeLabel2=Este asistente instalara [name/ver] en su equipo.%n%nLa aplicacion no necesita .NET instalado (viene completo), pero si necesita un servidor SQL Server 2022 accesible.%n%nSe recomienda cerrar cualquier otra aplicacion antes de continuar.
es.FinishedLabel=La instalacion termino.%n%nSi aun no configuro la base de datos, use el acceso "Configurar base de datos" del menu Inicio antes de abrir NamiSoft.
