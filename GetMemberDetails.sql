Create procedure [dbo].[AddMemberDetails]
(
	@tag varchar(50),
	@name varchar(50),
	@expLevel int,
	@leagueID int,
	@leagueName varchar(50),
	@leagueIcon varchar(50),
	@trophies int,
	@role varchar(50),
	@clanRank int,
	@donations int,
	@donationsReceived int,
	@versusTrophies int,	
	@dateNow date
)
as
begin
	insert into MemberList values(@tag, @name, @expLevel, @leagueID, @leagueName, @leagueIcon, @trophies,
	@role, @clanRank, @donations, @donationsReceived, @versusTrophies, @dateNow)
end