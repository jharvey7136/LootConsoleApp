Create procedure [dbo].[AddClanDetails]
(
	@tag varchar(15),
	@name varchar(20),
	@badgeUrls varchar(50),
	@clanLevel int,
	@clanPoints int,
	@members int,
	@warWinStreak int,
	@warWins int,
	@description varchar(MAX),
	@type varchar(50),
	@requiredTrophies int,
	@warFrequency varchar(50),
	@dateNow date
)
as
begin
	insert into ClanDetails values(@tag, @name, @badgeUrls, @clanLevel, @clanPoints, @members, @warWinStreak,
	@warWins, @description, @type, @requiredTrophies, @warFrequency, @dateNow)
end