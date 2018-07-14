Loot Console App
===============

## John Harvey 

Windows Console Application to Fetch data through the CoC API. For self use, you must go into Program.cs, rename the playertag string with your string, and reconfigure to 
use with your database. This is for those looking for a simple template to fetch data from the CoC API. Credit goes to [smietanka](https://github.com/smietanka)
for the CoCNET wrapper.

### Install Notes
1. Windows required, Visual Studio to rename/rebuild
2. [Clone repo](https://github.com/jharvey7136/LootConsoleApp.git)
3. Rename/Configure Program.cs to your liking
4. Build to either bin/debug or bin/release
5. Run `LootConsoleApp.exe` from bin/debug or bin/release, which ever you built to
6. For scheduled use, create a new task with Windows Task Scheduler

### Example Usage
Schedule this console app to run every day at 11:55pm. By collecting data at the end of every day and storing records to a database, over time you will
gather enough data to model it and analyze trends.

#### Resources
[CoCNET Wrapper](https://github.com/smietanka/CocNET)

[Clash of Clans API](https://developer.clashofclans.com)