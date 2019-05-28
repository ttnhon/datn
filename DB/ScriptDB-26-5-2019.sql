CREATE DATABASE CODING_CHALLENGE
GO 
USE CODING_CHALLENGE

CREATE TABLE SCHOOL(
	ID				int					NOT NULL IDENTITY primary key,
	Name			nvarchar(100)		NOT NULL UNIQUE,
    Description		text				
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
	 About 			text			,
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
	TimeEnd			   datetime,							--Thời gian kết thúc compete
	ParticipantCount   int					NOT NULL		--Số người tham gia vào compete

	foreign key (OwnerID) references  USER_INFO(ID)
);

CREATE TABLE CHALLENGE(
	ID					    int				    NOT NULL IDENTITY primary key,
	OwnerID			        int				    NOT NULL,		--Người chủ sở hữu
	Title				    nvarchar(256)		NOT NULL,		--tiêu đề
	Slug				    varchar(256)		NOT NULL,		--tieu-de
	Description			    text			    NOT NULL,		--mô tả challenge
	ProblemStatement		nvarchar(256),						--
	InputFormat			    text			,				    --input test case
	OutputFormat		    text			,				    --output test case
	ChallengeDifficulty		smallint			NOT NULL,		--độ khó
	Constraints			    nvarchar(256)	,					--chưa biết làm gì (đẻ tạm)
	TimeDo				    int				,				    --thời gian làm bài, optional
	Score				    int				    NOT NULL,		--điểm đạt được nếu hoàn thành
	Solution			    text			,				    --giải pháp làm bài nếu "bí"
	Tags				    nvarchar(256)	,					--Các thẻ (phụ vụ cho việc tìm kiếm (nếu có))
	LanguageCSharp bit,
	LanguageCpp bit,
	LanguageJava bit,
	DisCompileTest bit,
	DisCustomTestcase bit,
	DisSubmissions bit,
	PublicTestcase bit,
	PublicSolutions bit,
	RequiredKnowledge nvarchar(256),
	TimeComplexity nvarchar(256),
	Editorialist nvarchar(256),
	PartialEditorial nvarchar(256),
	Approach nvarchar(256),
	ProblemSetter nvarchar(256),
	SetterCode nvarchar(256),
	ProblemTester nvarchar(256),
	TesterCode nvarchar(256),

	foreign key (OwnerID) references  USER_INFO(ID)
);

CREATE TABLE CHALLENGE_LANGUAGE(		--Lưu các ngôn ngữ của 1 challenge
  ID            int       NOT NULL IDENTITY,
  ChallengeID   int       NOT NULL,
  LanguageID    int		  NOT NULL,
  CodeStub ntext,

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
	TimeDone			datetime		NOT NULL,		--thời gian nộp bài

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

--Create data
--SCHOOL
INSERT INTO SCHOOL VALUES (N'Khác', N'Mô tả trường thôi')
INSERT INTO SCHOOL VALUES (N'Đại học Khoa học Tự nhiên TPHCM', N'Mô tả trường thôi')
INSERT INTO SCHOOL VALUES (N'Đại học Bách khoa TPHCM', N'Mô tả trường thôi')
INSERT INTO SCHOOL VALUES (N'Đại học Khoa học Tự nhiên Hà Nội', N'Mô tả trường thôi')
INSERT INTO SCHOOL VALUES (N'Đại học bách khoa Hà Nội', N'Mô tả trường thôi')
GO
--USER_INFO
INSERT INTO USER_INFO VALUES (N'phucphieu', N'e10adc3949ba59abbe56e057f20f883e', N'Phuc', N'Phieu', N'phuc@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'phucoccho', N'e10adc3949ba59abbe56e057f20f883e', N'Phucvl', N'Phieu', N'phucvl@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'ngocdeptrai', N'e10adc3949ba59abbe56e057f20f883e', N'hihi', N'Phieu', N'ngocbui@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'longu', N'e10adc3949ba59abbe56e057f20f883e', N'longml', N'Phieu', N'longporm@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'myduyen', N'e10adc3949ba59abbe56e057f20f883e', N'Duyen', N'Phieu', N'myduyen@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'nghiaNgu', N'e10adc3949ba59abbe56e057f20f883e', N'nguvl', N'Phieu', N'nghiangu@gmail.com', 1, N'''photoURL''', 1,'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())

--LANGUAGE
INSERT INTO LANGUAGE VALUES (N'Cpp', N'abc')
INSERT INTO LANGUAGE VALUES (N'CSharp', N'Create data migrate')
INSERT INTO LANGUAGE VALUES (N'Java', N'Create data migrate')

--COMPETE
INSERT INTO COMPETE VALUES(1,N'Project code challenge 1', 'project-code-challenge-1', N'Chỉ là mô tả thôi 0', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 2', 'project-code-challenge-2', N'Chỉ là mô tả thôi 1', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 3', 'project-code-challenge-3', N'Chỉ là mô tả thôi 2', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 4', 'project-code-challenge-4', N'Chỉ là mô tả thôi 3', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 5', 'project-code-challenge-5', N'Chỉ là mô tả thôi 4', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 6', 'project-code-challenge-6', N'Chỉ là mô tả thôi 5', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 7', 'project-code-challenge-7', N'Chỉ là mô tả thôi 6', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 8', 'project-code-challenge-8', N'Chỉ là mô tả thôi 7', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 9', 'project-code-challenge-9', N'Chỉ là mô tả thôi 8', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)

--CHALLENGE
INSERT INTO CHALLENGE VALUES (1, N'Print Hello world! Java', N'''print-hello-world''', N'''Tét thôi ghi đại di''', N'''hahaha''', N'''hahaha''', 1, N'1', 60, 100, N'''tét thôi''', N'''hahahahaha''')
INSERT INTO CHALLENGE VALUES (2, N'Array sum Java', N'array-sum-java', N'test 2', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt')
INSERT INTO CHALLENGE VALUES (3, N'test number 3', N'test-number-3', N'test 3', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt')
INSERT INTO CHALLENGE VALUES (4, N'test number 4', N'test-number-4', N'test 4', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt')
INSERT INTO CHALLENGE VALUES (5, N'test number 5', N'test-number-5', N'test 5', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt')
INSERT INTO CHALLENGE VALUES (6, N'test number 6', N'test-number-2', N'test 2', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt')
INSERT INTO CHALLENGE VALUES (1, N'test number 7', N'test-number-3', N'test 3', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt')
INSERT INTO CHALLENGE VALUES (2, N'test number 8', N'test-number-4', N'test 4', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt')
INSERT INTO CHALLENGE VALUES (3, N'test number 9', N'test-number-5', N'test 5', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt')
INSERT INTO CHALLENGE VALUES (4, N'test number 10', N'test-number-2', N'test 2', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt')
INSERT INTO CHALLENGE VALUES (5, N'test number 11', N'test-number-3', N'test 3', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt')
INSERT INTO CHALLENGE VALUES (6, N'test number 12', N'test-number-4', N'test 4', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt')
INSERT INTO CHALLENGE VALUES (1, N'test number 13', N'test-number-5', N'test 5', N'fadfd', N'sfgfgf', 1, N'1', 60, 100, N'gdfgdfg', N'ttttt')

--ANSWER
INSERT INTO ANSWER VALUES (3, 1, N'sgsdgf', 1, GETDATE())
INSERT INTO ANSWER VALUES (4, 1, N'rtrte', 1, GETDATE())

--CHANLLENGE_COMPETE
INSERT INTO CHALLENGE_COMPETE VALUES (1, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (2, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (3, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (4, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (5, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (6, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (7, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (8, 1)

--CHALLENGE_LANGUAGE
INSERT INTO CHALLENGE_LANGUAGE VALUES (1, 3)
INSERT INTO CHALLENGE_LANGUAGE VALUES (2, 3)
INSERT INTO CHALLENGE_LANGUAGE VALUES (2, 1)
INSERT INTO CHALLENGE_LANGUAGE VALUES (2, 2)
INSERT INTO CHALLENGE_LANGUAGE VALUES (3, 3)
INSERT INTO CHALLENGE_LANGUAGE VALUES (4, 3)
INSERT INTO CHALLENGE_LANGUAGE VALUES (5, 3)
INSERT INTO CHALLENGE_LANGUAGE VALUES (6, 3)

--CHALLENGE_EDITOR
INSERT INTO CHALLENGE_EDITOR VALUES (1, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (1, 2)
INSERT INTO CHALLENGE_EDITOR VALUES (1, 3)
INSERT INTO CHALLENGE_EDITOR VALUES (1, 4)
INSERT INTO CHALLENGE_EDITOR VALUES (2, 1)

--TEST_CASE
INSERT INTO TESTCASE VALUES (1, 'challenge_1_input_0.txt', 'challenge_1_output_0.txt')
INSERT INTO TESTCASE VALUES (1, 'challenge_1_input_1.txt', 'challenge_1_output_1.txt')
INSERT INTO TESTCASE VALUES (1, 'challenge_1_input_2.txt', 'challenge_1_output_2.txt')
INSERT INTO TESTCASE VALUES (2, 'challenge_2_input_0.txt', 'challenge_2_output_0.txt')
INSERT INTO TESTCASE VALUES (2, 'challenge_2_input_1.txt', 'challenge_2_output_1.txt')
INSERT INTO TESTCASE VALUES (2, 'challenge_2_input_2.txt', 'challenge_2_output_2.txt')

--COMMENT
INSERT INTO COMMENT ([ID], [Text], [CreateDate], [Likes], [OwnerID], [ChallengeID]) VALUES (1, N'Comment choi thoi', CAST(N'2019-01-01 00:00:00.000' AS DateTime), 1, 2, 2)

--REPLY
INSERT INTO REPLY ([ID], [CommentID], [OwnerID], [Text], [CreateDate], [Likes]) VALUES (1, 1, 2, N'Reply choi thoi', CAST(N'2019-01-10 00:00:00.000' AS DateTime), 10)