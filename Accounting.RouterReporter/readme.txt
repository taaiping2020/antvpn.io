sc.exe create RouterReporter binPath="c:\rr\Accounting.RouterReporter.exe" start="auto" depend= "RaMgmtSvc"
Start-Service RouterReporter
Get-Service RouterReporter

Get-Service RouterReporter | Stop-Service
sc.exe delete RouterReporter
Get-Service RouterReporter

curl http://bosxixi.com/temp/rr.zip -Outfile c:/rr.zip


{
    "server": "0.0.0.0",
    "port_password": {
        "65535": "sdfsdfasfcvcf41545dfg"
    },
    "timeout": 300,
    "method": "aes-256-cfb"
}

ssserver --manager-address 0.0.0.0:6001 -c c:/mu.json

ssserver --manager-address 127.0.0.1:6001 -c c:/mu.json