# Important

## This repository is archived, as this application has been refactored into [Plpext](https://github.com/cotti/plpext). It's now somewhat better!

# **MessengerPlusSoundBankExtractor**
---
![image](https://user-images.githubusercontent.com/889227/225746157-18848bb9-83b7-4718-a9d7-5ec578a07041.png)

https://user-images.githubusercontent.com/889227/226512459-78bbc8e7-625b-4c2e-a97c-8738cf9686c0.mp4

#### _A MSN Messenger Plus sound packs extractor (.plp)_
[![.NET Core Desktop](https://github.com/cotti/MessengerPlusSoundBankExtractor/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/cotti/MessengerPlusSoundBankExtractor/actions/workflows/dotnet-desktop.yml)
---

### *How does this run?*

It just blops the contents in memory and allows you to preview the audios.

### *How does one run this?*

The one point of contention would be OpenAL32.dll. I have added it to the release, but in case it doesn't work for your setup, you can get it here (Windows Installer) https://openal.org/downloads/

OpenAL should be a widespread library on GNU/Linux, so just get it before running this.

Now, for actually running... It _should_ be something like

` dotnet run --project MessengerPlusSoundBankExtractor/MessengerPlusSoundBankExtractor.csproj`

### *What is the license on this thing?*

GNU AGPL v3. Hopefully.
