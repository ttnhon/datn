﻿
CREATE DATABASE CODING_CHALLENGE
GO 
USE CODING_CHALLENGE

CREATE TABLE USER_INFO(
	 ID				int				NOT NULL IDENTITY PRIMARY KEY,
	 UserName		nvarchar(50)	NOT NULL UNIQUE,
	 PasswordUser	nvarchar(50)	NOT NULL,
	 FirstName		nvarchar(50)	NOT NULL,
	 LastName		nvarchar(50)	NOT NULL,
	 Email			nvarchar(50)	NOT NULL,
	 StatusUser		int				,          --Trạng thái (tạm)
	 RoleUser		int				,          --Phân quyền user (user, moderator, admin)
	 PhotoURL		varchar(256)	,          --Đường link đến ảnh đại diện
	 CreateDate		timestamp		NOT NULL   --Ngày tạo tài khoản
);

CREATE TABLE LANGUAGE(               --Lưu trữ các ngôn ngữ mà trang web hỗ trợ (C++, Java, CSharp)
	ID				int				    NOT NULL IDENTITY primary key,
	Name			nvarchar(100)		NOT NULL UNIQUE,
	Description		text
);

CREATE TABLE COMPETE(				
	ID				   int				    NOT NULL IDENTITY primary key,
	OwnerID			   int				    NOT NULL,		--Chủ sở hữu
	Title			   nvarchar(256)		NOT NULL,		--Tiêu đề
	Slug			   varchar(256)			NOT NULL,		--tieu-de   (dùng để tạo link thân thiện)
	Description		   text,								--Mô tả
	Rules			   nvarchar(256),						--Chưa rõ
	TotalScore		   int				    NOT NULL,		--Tổng điểm của compete
	TimeEnd			   timestamp,							--Thời gian kết thúc compete
	ParticipantCount   int					NOT NULL		--Số người tham gia vào compete

	foreign key (OwnerID) references  USER_INFO(ID)
);

CREATE TABLE CHALLENGE(
	ID					    int				    NOT NULL IDENTITY primary key,
	OwnerID			        int				    NOT NULL,		--Người chủ sở hữu
	Title				    nvarchar(256)		NOT NULL,		--tiêu đề
	Slug				    varchar(256)		NOT NULL,		--tieu-de
	Description			    text			    NOT NULL,		--mô tả challenge
	InputFormat			    text			,				    --input test case
	OutputFormat		    text			,				    --output test case
	ChallengeDifficulty		smallint			NOT NULL,		--độ khó
	Constraints			    nvarchar(256)	,					--chưa biết làm gì (đẻ tạm)
	TimeDo				    int				,				    --thời gian làm bài, optional
	Score				    int				    NOT NULL,		--điểm đạt được nếu hoàn thành
	Solution			    text			,				    --giải pháp làm bài nếu "bí"
	Tags				    nvarchar(256)	,					--Các thẻ (phụ vụ cho việc tìm kiếm (nếu có))
	Languages			    varchar(256)						--Các ngôn ngữ, dịnh dạng json   ví dụ: {"Java", "CSharp"}

	foreign key (OwnerID) references  USER_INFO(ID)
);

CREATE TABLE CHALLENGE_LANGUAGE(		--Lưu các ngôn ngữ của 1 challenge
  ID            int       NOT NULL IDENTITY,
  ChallengeID   int       NOT NULL,
  LanguageID    int		  NOT NULL,

  PRIMARY KEY (ChallengeID, LanguageID),
  foreign key (ChallengeID) references  CHALLENGE(ID),
  foreign key (LanguageID)	references  LANGUAGE(ID)
);
--Bảng có chỉnh sửa (Bỏ ID làm khóa chính)
CREATE TABLE CHALLENGE_COMPETE( --Lưu trữ quan hệ giữ Challenge với compete    ( N - N )
  ID            int       NOT NULL IDENTITY,	--xác định vị trí của challenge trong compete
  ChallengeID   int       NOT NULL,
  CompeteID     int       NOT NULL,
  --SerialNumber  int       NOT NULL			--Đã bỏ do có thể dùng ID để biết được số thứ tự của challenge trong compete
  PRIMARY KEY (ChallengeID, CompeteID),
  foreign key (ChallengeID) references  CHALLENGE(ID),
  foreign key (CompeteID)	references  COMPETE(ID)
);

CREATE TABLE CHALLENGE_EDITOR( --Lưu trữ quan hệ giữ Challenge với Editor   ( N - N )      1 challenge có thể có nhiều editor và ngược lại
	ID				    int				NOT NULL IDENTITY,
	ChallegenID			int				NOT NULL,
	EditorID			int				NOT NULL,

	primary key (ChallegenID, EditorID),
	foreign key (ChallegenID)	references  CHALLENGE(ID),
	foreign key (EditorID)		references  USER_INFO(ID)
);

CREATE TABLE TESTCASE(  --Lưu trữ test case cho challenge     (1 challenge có thể có 1 hoặc nhiều testcase)
	ID				    int				    NOT NULL IDENTITY primary key,
	ChallengeID			int				    NOT NULL,
	Input			    varchar(256)		NOT NULL,
	Output				varchar(256)		NOT NULL,

	foreign key (ChallengeID) references  CHALLENGE(ID)
);

CREATE TABLE ANSWER(    --Lưu trữ câu trả lời của user với từng challenge
	ID				    int				NOT NULL IDENTITY primary key,
	ChallengeID			int				NOT NULL,
	UserId				int				NOT NULL,
	Content				text			NOT NULL,		--lời giải
	Result				bit				NOT NULL,		--kết quả câu trả lời
	TimeDone			timestamp		NOT NULL,		--thời gian nộp bài

	foreign key (ChallengeID)	references CHALLENGE(ID),
	foreign key (UserId)		references USER_INFO(ID)
);

CREATE TABLE ADD_DATA(	--Dùng để lưu trữ những dữ liệu nhỏ, lẻ tẻ
	ID				    int				NOT NULL IDENTITY primary key,
	Title				varchar(256)	NOT NULL,
	Data			    text			NOT NULL        --dạng json (mảng dữ liệu)
);

CREATE TABLE USER_DATA(	--Dùng để lưu trữ dữ liệu của người dùng: process status, event status, ....
	UserID				int             NOT NULL primary key,
	Title				varchar(256)	NOT NULL,
	Data			    text			NOT NULL      --dạng json (mảng dữ liệu)

	foreign key (UserId)		references USER_INFO(ID)
);

--Create data
--USER_INFO
INSERT INTO USER_INFO VALUES (N'phucphieu', N'e10adc3949ba59abbe56e057f20f883e', N'Phuc', N'Phieu', N'phuc@gmail.com', 1, 1, N'''hahaha''',DEFAULT)
INSERT INTO USER_INFO VALUES (N'phucoccho', N'e10adc3949ba59abbe56e057f20f883e', N'Phucvl', N'Phieu', N'phucvl@gmail.com', 1, 1, N'''hahaha''',DEFAULT)
INSERT INTO USER_INFO VALUES (N'ngocdeptrai', N'e10adc3949ba59abbe56e057f20f883e', N'hihi', N'Phieu', N'ngocbui@gmail.com', 1, 1, N'''hahaha''',DEFAULT)
INSERT INTO USER_INFO VALUES (N'longu', N'e10adc3949ba59abbe56e057f20f883e', N'longml', N'Phieu', N'longporm@gmail.com', 1, 1, N'''hahaha''',DEFAULT)
INSERT INTO USER_INFO VALUES (N'myduyen', N'e10adc3949ba59abbe56e057f20f883e', N'Duyen', N'Phieu', N'myduyen@gmail.com', 1, 1, N'''hahaha''',DEFAULT)
INSERT INTO USER_INFO VALUES (N'nghiaNgu', N'e10adc3949ba59abbe56e057f20f883e', N'nguvl', N'Phieu', N'nghiangu@gmail.com', 1, 1, N'''hahaha''',DEFAULT)
--LANGUAGE
INSERT INTO LANGUAGE VALUES (N'C++', N'abc')
INSERT INTO LANGUAGE VALUES (N'Cpp', N'fsdfd')
INSERT INTO LANGUAGE VALUES (N'CSharp', N'Create data migrate')
INSERT INTO LANGUAGE VALUES (N'Java', N'Create data migrate')

--CHALLENGE
INSERT INTO CHALLENGE VALUES (1, N'''Đây là challenge test''', N'''day-la-challenge-test''', N'''Tét thôi ghi d?i di''', N'''hahaha''', N'''hahaha''', 1, N'1', 60, 100, N'''tét thôi''', N'''hahahahaha''', N'''{"Cpp"}''')
INSERT INTO CHALLENGE VALUES (1, N'test number 2', N'test-number-2', N'test 2', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt', N'{"CSharp"}')
INSERT INTO CHALLENGE VALUES (1, N'test number 3', N'test-number-3', N'test 3', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt', N'{"CSharp"}')
INSERT INTO CHALLENGE VALUES (1, N'test number 4', N'test-number-4', N'test 4', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt', N'{"CSharp"}')
INSERT INTO CHALLENGE VALUES (1, N'test number 5', N'test-number-5', N'test 5', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt', N'{"CSharp"}')

--ANSWER
INSERT INTO ANSWER VALUES (3, 1, N'sgsdgf', 1, DEFAULT)
INSERT INTO ANSWER VALUES (4, 1, N'rtrte', 1, DEFAULT)

--CHANLLENGE_COMPETE


