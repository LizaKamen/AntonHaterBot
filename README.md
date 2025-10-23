bot hater

disable privacy mode (Bot settings -> Group privacy -> Turn off)
```
git clone https://github.com/LizaKamen/AntonHaterBot.git
cd AntonHaterBot
mv appsettings.example.json appsettings.json
# change token,username and emoji with preferred values 
docker build -t hate-bot .
docker run -d -i hate-bot
```