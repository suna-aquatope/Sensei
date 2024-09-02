# SCHALE.GameServer

## Attention

frida method does not work in the latest version of the game

use the mitmproxy method

usage is in [README](https://github.com/Endergreen12/SCHALE.GameServer/blob/master/Scripts/redirect_server_mitmproxy/README.md)

---

I make destructive changes so often that I recommend deleting the bin folder and

deleting the account and starting over completely after updating the server code

---

## Differences from original repo

### Fixed Features

- Working formation screen

![image](https://github.com/user-attachments/assets/33c98ed2-8124-4e28-9842-00fe3eb69872)

The reason for this bug is that incorrect echelon data was added when the account was created

Therefore, if you created your account before this fix was added, you will need to delete and recreate your account or delete the incorrect data

The incorrect data should be at the very beginning of echelon table

---

### Implemented Features

- Campaign (Currently strategy skip mode only)

![image](https://github.com/user-attachments/assets/18feabe9-3013-4bfc-9ddf-d14bb6b6cbbe)

![image](https://github.com/user-attachments/assets/4f446aea-30a9-4ddf-aa51-daf930a5cb28)

![image](https://github.com/user-attachments/assets/6e91eafa-00dc-41c9-a418-ad2d32cdc386)

---

- Unlock All Command

`/unlockall campaign` to unlock all of stages of campaign

---

- Support for loading excel of latest version of game

![image](https://github.com/user-attachments/assets/2487164d-b56b-433b-a500-c6bd670c4f59)

With this support, “/character add all” and “/inventory addall” commands will now properly add the latest characters and memory lobbies

If you built the server before this support was added, please delete the bin folder and completely rebuild it

---

- Fixed Gacha

![image](https://github.com/user-attachments/assets/30f12db7-5405-4a11-9576-6a71ddb9c54f)

---

- Week Dungeon (Bounty), School Dungeon(Scrimmage)

![image](https://github.com/user-attachments/assets/0c773325-00b2-48fb-b24d-b617f40352cd)

![image](https://github.com/user-attachments/assets/abd58db4-276e-4a2b-8096-c96bd8753890)

![image](https://github.com/user-attachments/assets/661aa5a0-1ae7-4d7b-bf81-9a30fd026f93)

---

- Currencies

![image](https://github.com/user-attachments/assets/e6340e55-2296-455f-a606-8e5062f67781)

This makes it behave the same as the game, e.g., when you pull a gacha, the gems are reduced

---

- Currency Command

![image](https://github.com/user-attachments/assets/b9f9d43a-7d0e-40d9-a6a4-8c836b19403a)

![image](https://github.com/user-attachments/assets/6e62d283-6edd-424b-920d-3767549e9ba0)

currencyId can be found at [SCHALE.Common/FlatData/CurrencyTypes.cs](https://github.com/Endergreen12/SCHALE.GameServer/blob/master/SCHALE.Common/FlatData/CurrencyTypes.cs)

---

- Cafe

![image](https://github.com/user-attachments/assets/4f8cf801-79a8-4712-b009-098aa55d9dd9)

As you can see, you can enter cafe, but it is broken

## Special Thanks

- K0lb3 - Author of Flatbuffer schema generator

- suna-aquatope - Made me realize how to do Json deserialization of GroupSummary
