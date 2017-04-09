sc.exe create RouterReporter binPath="c:\rr\Accounting.RouterReporter.exe" start="auto" depend= "RaMgmtSvc"
Start-Service RouterReporter
Get-Service RouterReporter

Get-Service RouterReporter | Stop-Service
sc.exe delete RouterReporter
Get-Service RouterReporter

curl http://bosxixi.com/temp/rr.zip -Outfile c:/rr.zip