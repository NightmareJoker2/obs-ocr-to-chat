OBS OCR to Chat
===

A small application that enables detection of, optical character recoginition of, and posting of the discovered text into Twitch chat.
The functionality is very rudimentary, somewhat janky, and for simplicity, the signed in user's channel is used.

How it works
---

The application uses [obs-websocket-dotnet](https://github.com/BarRaider/obs-websocket-dotnet) to connect to the [obs-websocket](https://github.com/Palakis/obs-websocket) server in OBS to grab a screenshot, crops the image to the area of interest, passes the result into [tesseract](https://github.com/charlesw/tesseract) for OCR. If the message is different from the previous one it will be sent into chat.

License
---
This project is licensed under the [GNU Affero Public License 3.0](https://www.gnu.org/licenses/agpl-3.0.en.html "GNU Affero General Public License - GNU Project - Free Software Foundation") (AGPL). What does this mean? It means you are free do do with this software what you want, but if you make changes to its source code, you must publish your changes. It also means there is no warranty, not even the implied warranty that the software does what it intends to do. You are solely responsible for your use of the software. Use at your own risk.

