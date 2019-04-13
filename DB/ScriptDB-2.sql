--use online_test_code
--DROP DATABASE CODING_CHALLEGEN
--GO
CREATE DATABASE CODING_CHALLEGEN
GO 
USE CODING_CHALLEGEN

CREATE TABLE USER_INFO(
	 ID				      int				NOT NULL IDENTITY PRIMARY KEY,
	 UserName		    nvarchar(50)	NOT NULL UNIQUE,
	 PasswordUser	  nvarchar(50)	NOT NULL,
	 FirstName		  nvarchar(50)	NOT NULL,
	 LastName		    nvarchar(50)	NOT NULL,
	 Email			    nvarchar(50)	NOT NULL,
	 StatusUser		  int				,
	 RoleUser		    int				,
	 PhotoURL		    varchar(256)	,
	 CreateDate		  timestamp		NOT NULL
)

CREATE TABLE LANGUAGE_CODE(
	ID				      int				    NOT NULL IDENTITY primary key,
	Name			      nvarchar(100)	NOT NULL,
	Description		  text
);

CREATE TABLE COMPETE(				
	ID				        int				    NOT NULL IDENTITY primary key,
	OwnerID			      int				    NOT NULL,
	Title			        nvarchar(256)	NOT NULL,
	Description		    text,
	Rules			        nvarchar(256),
	TotalScore		    int				    NOT NULL,
	ParticipantCount  int           NOT NULL

	foreign key (OwnerID) references  USER_INFO(ID)
);
CREATE TABLE CHALLENGE(
	ID					        int				    NOT NULL IDENTITY primary key,
	OwnerID			        int				    NOT NULL,
	Title				        nvarchar(256)	NOT NULL,		--tiêu đề
	Slug				        varchar(256)	NOT NULL,		--tieu-de
	Description			    text			    NOT NULL,		--mô tả challegen
	InputFormat			    text			,				--input test case
	OutputFormat		    text			,				--output test case
	ChallengeDifficulty	smallint		  NOT NULL,		--độ khó
	Constraints			    nvarchar(256)	,
	TimeDo				      int				,				--thời gian làm bài, optional
	Score				        int				    NOT NULL,		--điểm đạt được nếu hoàn thành
	Solution			      text			,				--giải pháp làm bài nếu "bí"
	Tags				        nvarchar(256)	,
	Languages			      varchar(256)	NOT NULL		--Các ngôn ngữ, dịnh dạng json,

	foreign key (OwnerID) references  USER_INFO(ID)
);

CREATE TABLE CHALLENGE_IN_COMPETE(
  ID            int       NOT NULL IDENTITY primary key,
  ChallengeID   int       NOT NULL,
  CompeteID     int       NOT NULL,
  SerialNumber  int       NOT NULL

  foreign key (ChallengeID) references  CHALLENGE(ID),
  foreign key (CompeteID) references  COMPETE(ID)
)
CREATE TABLE CHALLENGE_EDITOR(
	ID				    int				NOT NULL IDENTITY primary key,
	ChallegenID		int				NOT NULL,
	EditorID		  int				NOT NULL,

	foreign key (ChallegenID) references  CHALLENGE(ID),
	foreign key (EditorID) references  USER_INFO(ID)
);

CREATE TABLE TESTCASE(
	ID				    int				    NOT NULL IDENTITY primary key,
	ChallengeID		int				    NOT NULL,
	Input			    varchar(256)	NOT NULL,
	Output			  varchar(256)	NOT NULL,

	foreign key (ChallengeID) references  CHALLENGE(ID)
);

CREATE TABLE ANSWER(
	ID				    int				NOT NULL IDENTITY primary key,
	ChallengeID		int				NOT NULL,
	UserId			  int				NOT NULL,
	Content			  text			NOT NULL,		--lời giải
	Result			  bit				NOT NULL,		--kết quả câu trả lời
	TimeDone      timestamp NOT NULL,   --thời gian nộp bài

	foreign key(ChallengeID) references CHALLENGE(ID),
	foreign key(UserId)		 references USER_INFO(ID)
)