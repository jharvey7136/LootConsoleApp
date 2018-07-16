Create procedure [dbo].[AddWarLog]
(
	@result varchar(50),
	@endTime varchar(50),
	@teamSize int,
	@clanTag varchar(50),
	@clanName varchar(50),
	@clanBadge varchar(50),
	@clanLevel int,
	@attacks int,
	@stars int,
	@expEarned int,
	@oppTag varchar(50),
	@oppName varchar(50),
	@oppBadge varchar(50),
	@oppClanLevel int,
	@oppAttacks int,
	@oppStars int,
	@oppExpEarned int,
	@dateNow date
)
as
begin
	insert into WarLog values(@result, @endTime, @teamSize, @clanTag, @clanName, @clanBadge, @clanLevel, @attacks, @stars,
	@expEarned, @oppTag, @oppName, @oppBadge, @oppClanLevel, @oppAttacks, @oppStars, @oppExpEarned, @dateNow)
end