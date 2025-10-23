bot hater

disable privacy mode (Bot settings -> Group privacy -> Turn off)
```
git clone https://github.com/LizaKamen/AntonHaterBot.git
cd AntonHaterBot
mv AntonHateBot/appsettings.example.json AntonHateBot/appsettings.json
# change appsettings with preferred values 
docker build -t hate-bot .
docker run -d -i hate-bot
```