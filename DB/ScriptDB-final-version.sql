CREATE DATABASE CODING_CHALLENGE
GO 
USE CODING_CHALLENGE

CREATE TABLE SCHOOL(
	ID				int					NOT NULL IDENTITY primary key,
	Name			nvarchar(100)		NOT NULL UNIQUE,
    Description		ntext				
);

CREATE TABLE USER_INFO(
	 ID				int				NOT NULL IDENTITY PRIMARY KEY,
	 UserName		nvarchar(50)	NOT NULL UNIQUE,
	 PasswordUser	nvarchar(50)	NOT NULL,
	 FirstName		nvarchar(50)	NOT NULL,
	 LastName		nvarchar(50)	NOT NULL,
	 Email			nvarchar(50)	NOT NULL,
	 RoleUser		int				NOT NULL,          --Phân quyền user (user, moderator, admin)
	 PhotoURL		varchar(256)	,          --Đường link đến ảnh đại diện
	 StatusUser		int				,          --Trạng thái (tạm)
     Country		nvarchar(50)	,
	 About 			ntext			,
	 SchoolID		int				NOT NULL,
     YearGraduation	int				,
     FacebookLink 	varchar(256)	,
	 GoogleLink 	varchar(256)	,
     CreateDate		datetime		NOT NULL,   --Ngày tạo tài khoản

	 foreign key (SchoolID) references SCHOOL(ID)
);

CREATE TABLE LANGUAGE(               --Lưu trữ các ngôn ngữ mà trang web hỗ trợ (C++, Java, CSharp)
	ID				int				    NOT NULL IDENTITY primary key,
	Name			nvarchar(100)		NOT NULL UNIQUE,
	Description		ntext
);

CREATE TABLE COMPETE(				
	ID				   int				    NOT NULL IDENTITY primary key,
	OwnerID			   int				    NOT NULL,		--Chủ sở hữu
	Title			   nvarchar(256)		NOT NULL,		--Tiêu đề
	Slug			   varchar(256)			NOT NULL,		--tieu-de   (dùng để tạo link thân thiện)
	Description		   ntext,								--Mô tả
	Rules			   nvarchar(256),						--Chưa rõ
	TotalScore		   int				    NOT NULL,		--Tổng điểm của compete
	TimeEnd			   datetime,							--Thời gian kết thúc compete
	IsPublic		   bit					NOT NULL,		--

	foreign key (OwnerID) references  USER_INFO(ID)
);

CREATE TABLE CHALLENGE(
	ID					    int				    NOT NULL IDENTITY primary key,
	OwnerID			        int				    NOT NULL,		--Người chủ sở hữu
	Title				    nvarchar(256)		NOT NULL,		--tiêu đề
	Slug				    varchar(256)		NOT NULL,		--tieu-de
	Description			    ntext			    NOT NULL,		--mô tả challenge
	ProblemStatement		nvarchar(256),						--
	InputFormat			    ntext			,				    --input test case
	OutputFormat		    ntext			,				    --output test case
	ChallengeDifficulty		smallint			NOT NULL,		--độ khó
	Constraints			    nvarchar(256)	,					--chưa biết làm gì (đẻ tạm)
	TimeDo				    int				,				    --thời gian làm bài, optional
	Score				    int				    NOT NULL,		--điểm đạt được nếu hoàn thành
	Solution			    ntext			,				    --giải pháp làm bài nếu "bí"
	Tags				    nvarchar(256)	,					--Các thẻ (phụ vụ cho việc tìm kiếm (nếu có))
	IsPublic				bit					NOT NULL,--Chế độ public hay private của challenge
	DisCompileTest bit,	--Tắt chức năng run challenge
	DisSubmissions bit,	--tắt chức năng submit challenge
	PublicSolutions bit,--public cách giải challenge sau khi submit

	foreign key (OwnerID) references  USER_INFO(ID)
);

CREATE TABLE CHALLENGE_LANGUAGE(		--Lưu các ngôn ngữ của 1 challenge
  ID            int       NOT NULL IDENTITY,
  ChallengeID   int       NOT NULL,
  LanguageID    int		  NOT NULL,
  CodeStub		ntext,

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
	Content				ntext			NOT NULL,		--lời giải
	Result				bit				NOT NULL,		--kết quả câu trả lời
	TimeDone			datetime		NOT NULL,		--thời gian nộp bài

	foreign key (ChallengeID)	references CHALLENGE(ID),
	foreign key (UserId)		references USER_INFO(ID)
);

CREATE TABLE ADD_DATA(	--Dùng để lưu trữ những dữ liệu nhỏ, lẻ tẻ
	ID				    int				NOT NULL IDENTITY primary key,
	Title				varchar(256)	NOT NULL,
	Data			    varchar(256)			NOT NULL        --dạng json (mảng dữ liệu)
);

CREATE TABLE USER_DATA(	--Dùng để lưu trữ dữ liệu của người dùng: process status, event status, ....
	UserID				int             NOT NULL primary key,
	Title				varchar(256)	NOT NULL,
	Data			    varchar(256)			NOT NULL      --dạng json (mảng dữ liệu)

	foreign key (UserId)		references USER_INFO(ID)
);

CREATE TABLE COMMENT(
	[ID]				[int]			NOT NULL IDENTITY PRIMARY KEY,
	[Text]				[text]			NOT NULL,
	[CreateDate]		[datetime]		NOT NULL,
	[Likes]				[int]			NULL,
	[OwnerID]			[int]			NOT NULL,
	[ChallengeID]		[int]			NOT NULL,

	FOREIGN KEY (OwnerID)		REFERENCES USER_INFO(ID),
	FOREIGN KEY (ChallengeID)	REFERENCES CHALLENGE(ID)
);

CREATE TABLE REPLY(
	[ID]				[int] NOT NULL IDENTITY PRIMARY KEY,
	[CommentID]			[int] NOT NULL,
	[OwnerID]			[int] NOT NULL,
	[Text]				[text] NOT NULL,
	[CreateDate]		[datetime] NOT NULL,
	[Likes]				[int] NULL,

	FOREIGN KEY (OwnerID)		REFERENCES USER_INFO(ID),
	FOREIGN KEY (CommentID)		REFERENCES COMMENT(ID)
);

CREATE TABLE LIKE_STATUS(
	ID				int NOT NULL IDENTITY PRIMARY KEY,
	OwnerID			int NOT NULL,
	CommentID		int,
	ReplyID			int,

	foreign key(OwnerID) references USER_INFO(ID),
	foreign key(CommentID) references COMMENT(ID),
	foreign key(ReplyID) references REPLY(ID),
);

CREATE TABLE QUESTION(
	ID				INT NOT NULL IDENTITY PRIMARY KEY,
	CompeteID		INT NOT NULL,
	Title			NTEXT,
	Choise			NTEXT,					--JSON
	Type			SMALLINT,
	Score			INT,
	Result			VARCHAR(255),			--JSON

	foreign key(CompeteID) references COMPETE(ID)
)

CREATE TABLE QUESTION_ANSWER(    --Lưu trữ câu trả lời của user với từng question
	QuestionID			int				NOT NULL,
	UserId				int				NOT NULL,
	Content				ntext			NOT NULL,		--lời giải
	Result				int				NOT NULL,		--kết quả câu trả lời
	TimeDone			datetime		NOT NULL,		--thời gian nộp bài

	PRIMARY KEY (QuestionID, UserId),
	foreign key (QuestionID)	references QUESTION(ID),
	foreign key (UserId)		references USER_INFO(ID)
);

CREATE TABLE COMPETE_PARTICIPANTS(
	CompeteID			int				NOT NULL,
	UserID				int				NOT NULL,
	TimeJoined			datetime

	PRIMARY KEY (CompeteID, UserID),
	foreign key (CompeteID)	references COMPETE(ID),
	foreign key (UserId)	references USER_INFO(ID)
)




--Create data
--SCHOOL
INSERT INTO SCHOOL VALUES (N'Khác', N'Mô tả trường thôi')
INSERT INTO SCHOOL VALUES (N'Đại học Khoa học Tự nhiên TPHCM', N'Mô tả trường thôi')
INSERT INTO SCHOOL VALUES (N'Đại học Bách khoa TPHCM', N'Mô tả trường thôi')
INSERT INTO SCHOOL VALUES (N'Đại học Khoa học Tự nhiên Hà Nội', N'Mô tả trường thôi')
INSERT INTO SCHOOL VALUES (N'Đại học bách khoa Hà Nội', N'Mô tả trường thôi')
GO
--USER_INFO
INSERT INTO USER_INFO VALUES (N'moderator1', N'e10adc3949ba59abbe56e057f20f883e', N'moderator1', N'A', N'moderator1@gmail.com', 1, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'moderator2', N'e10adc3949ba59abbe56e057f20f883e', N'moderator2', N'A', N'moderator2@gmail.com', 1, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'moderator3', N'e10adc3949ba59abbe56e057f20f883e', N'moderator3', N'A', N'moderator3@gmail.com', 1, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'phucnv', N'e10adc3949ba59abbe56e057f20f883e', N'Phúc', N'Nguyễn Văn', N'nvpit97@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'phucdh', N'e10adc3949ba59abbe56e057f20f883e', N'Phúc', N'Đỗ Hồng', N'dohongphuc1997@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'ngocbn', N'e10adc3949ba59abbe56e057f20f883e', N'Ngọc', N'Bùi Như', N'buinhungoc97@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'phuchp', N'e10adc3949ba59abbe56e057f20f883e', N'Phúc', N'Huỳnh Phi', N'person7@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'nhontt', N'e10adc3949ba59abbe56e057f20f883e', N'Nhơn', N'Trương Thành', N'person8@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person9', N'e10adc3949ba59abbe56e057f20f883e', N'P9', N'Phieu', N'person9@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person10', N'e10adc3949ba59abbe56e057f20f883e', N'P10', N'Phieu', N'person10@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person11', N'e10adc3949ba59abbe56e057f20f883e', N'P11', N'Phieu', N'person11@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person12', N'e10adc3949ba59abbe56e057f20f883e', N'P12', N'Phieu', N'person12@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person13', N'e10adc3949ba59abbe56e057f20f883e', N'P13', N'Phieu', N'person13@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person14', N'e10adc3949ba59abbe56e057f20f883e', N'P14', N'Phieu', N'person14@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person15', N'e10adc3949ba59abbe56e057f20f883e', N'P15', N'Phieu', N'person15@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person16', N'e10adc3949ba59abbe56e057f20f883e', N'P16', N'Phieu', N'person16@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person17', N'e10adc3949ba59abbe56e057f20f883e', N'P17', N'Phieu', N'person17@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person18', N'e10adc3949ba59abbe56e057f20f883e', N'P18', N'Phieu', N'person18@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person19', N'e10adc3949ba59abbe56e057f20f883e', N'P19', N'Phieu', N'person19@gmail.com', 0, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'admin', N'e10adc3949ba59abbe56e057f20f883e', N'admin', N'administrator', N'admin@gmail.com', 2, NULL, 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
go

--LANGUAGE
INSERT INTO LANGUAGE VALUES (N'Cpp', N'abc')
INSERT INTO LANGUAGE VALUES (N'CSharp', N'Create data migrate')
INSERT INTO LANGUAGE VALUES (N'Java', N'Create data migrate')
go

--COMPETE
INSERT INTO COMPETE VALUES(1,N'Project code challenge 1', 'project-code-challenge-1', N'Chỉ là mô tả thôi 1', N'Đây là phần rule của compete', 10000, '07/31/2019', 0)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 2', 'project-code-challenge-2', N'Chỉ là mô tả thôi 2', N'Đây là phần rule của compete', 10000, '07/31/2019', 0)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 3', 'project-code-challenge-3', N'Chỉ là mô tả thôi 3', N'Đây là phần rule của compete', 10000, '07/31/2019', 0)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 4', 'project-code-challenge-4', N'Chỉ là mô tả thôi 4', N'Đây là phần rule của compete', 10000, '07/31/2019', 1)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 5', 'project-code-challenge-5', N'Chỉ là mô tả thôi 5', N'Đây là phần rule của compete', 10000, '07/31/2019', 1)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 6', 'project-code-challenge-6', N'Chỉ là mô tả thôi 6', N'Đây là phần rule của compete', 10000, '07/31/2019', 1)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 7', 'project-code-challenge-7', N'Chỉ là mô tả thôi 7', N'Đây là phần rule của compete', 10000, '07/31/2019', 1)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 8', 'project-code-challenge-8', N'Chỉ là mô tả thôi 8', N'Đây là phần rule của compete', 10000, '07/31/2019', 1)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 9', 'project-code-challenge-9', N'Chỉ là mô tả thôi 9', N'Đây là phần rule của compete', 10000, '07/31/2019', 1)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 10', 'project-code-challenge-10', N'Chỉ là mô tả thôi 10', N'Đây là phần rule của compete', 10000, '07/31/2019', 1)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 11', 'project-code-challenge-11', N'Chỉ là mô tả thôi 11', N'Đây là phần rule của compete', 10000, '07/31/2019', 1)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 12', 'project-code-challenge-12', N'Chỉ là mô tả thôi 12', N'Đây là phần rule của compete', 10000, '07/31/2019', 1)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 13', 'project-code-challenge-13', N'Chỉ là mô tả thôi 13', N'Đây là phần rule của compete', 10000, '07/31/2019', 1)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 14', 'project-code-challenge-14', N'Chỉ là mô tả thôi 14', N'Đây là phần rule của compete', 10000, '07/31/2019', 1)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 15', 'project-code-challenge-15', N'Chỉ là mô tả thôi 15', N'Đây là phần rule của compete', 10000, '07/31/2019', 1)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 16', 'project-code-challenge-16', N'Chỉ là mô tả thôi 16', N'Đây là phần rule của compete', 10000, '07/31/2019', 0)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 17', 'project-code-challenge-17', N'Chỉ là mô tả thôi 17', N'Đây là phần rule của compete', 10000, '07/31/2019', 0)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 18', 'project-code-challenge-18', N'Chỉ là mô tả thôi 18', N'Đây là phần rule của compete', 10000, '07/31/2019', 0)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 19', 'project-code-challenge-19', N'Chỉ là mô tả thôi 19', N'Đây là phần rule của compete', 10000, '07/31/2019', 0)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 20', 'project-code-challenge-20', N'Chỉ là mô tả thôi 20', N'Đây là phần rule của compete', 10000, '07/31/2019', 0)
go