CREATE DATABASE CODING_CHALLEGEN
GO 
USE CODING_CHALLEGEN

CREATE TABLE USER_INFO(
	 ID				int				NOT NULL IDENTITY PRIMARY KEY,
	 UserName		nvarchar(50)	NOT NULL UNIQUE,
	 PasswordUser	nvarchar(50)	NOT NULL,
	 FirstName		nvarchar(50)	NOT NULL,
	 LastName		nvarchar(50)	NOT NULL,
	 Email			nvarchar(50)	NOT NULL,
	 StatusUser		int				,
	 RoleUser		int				,
	 PhotoURL		varchar(256)	,
	 CreateDate		timestamp		NOT NULL
)
CREATE TABLE LANGUAGE_CODE(
	ID				int				NOT NULL IDENTITY primary key,
	Name			nvarchar(100)	NOT NULL,
	Describe		text
);

CREATE TABLE COMPETE(				
	ID				int				NOT NULL IDENTITY primary key,
	TeacherID		int				NOT NULL,
	Title			nvarchar(256)	NOT NULL,
	Describe		text

	foreign key (TeacherID) references  USER_INFO(ID)
)
CREATE TABLE CHALLENGE(
	ID				int				NOT NULL IDENTITY primary key,
	CompeteID		int				,				--có thể không thuộc compete nào
	Title			nvarchar(256)	NOT NULL,
	Problem			text			NOT NULL,
	LanguageCode	int				,
	InputQuestion	text			,				--input test case
	OutputQuestion	text			,				--output test case
	LevelChallenge	smallint		,				--độ khó
	TimeDo			int				,				--thời gian làm bài
	Score			int				NOT NULL,		--điểm đạt được nếu hoàn thành
	Solution		text			,				--giải pháp làm bài nếu "bí"

	foreign key(LanguageCode) references LANGUAGE_CODE(ID),
	foreign key(CompeteID) references COMPETE(ID)
)
CREATE TABLE ANSWER(
	ID				int				NOT NULL IDENTITY primary key,
	ChallengeID		int				NOT NULL,
	UserId			int				NOT NULL,
	Content			text			NOT NULL,		--lời giải
	Result			bit				NOT NULL,		--kết quả câu trả lời	

	foreign key(ChallengeID) references CHALLENGE(ID),
	foreign key(UserId)		 references USER_INFO(ID)
)