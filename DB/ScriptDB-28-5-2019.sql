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
	PartialEditorial bit,
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
--INSERT INTO USER_INFO VALUES (N'phucphieu', N'e10adc3949ba59abbe56e057f20f883e', N'Phuc', N'Phieu', N'phuc@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
--INSERT INTO USER_INFO VALUES (N'phucoccho', N'e10adc3949ba59abbe56e057f20f883e', N'Phucvl', N'Phieu', N'phucvl@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
--INSERT INTO USER_INFO VALUES (N'ngocdeptrai', N'e10adc3949ba59abbe56e057f20f883e', N'hihi', N'Phieu', N'ngocbui@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
--INSERT INTO USER_INFO VALUES (N'longu', N'e10adc3949ba59abbe56e057f20f883e', N'longml', N'Phieu', N'longporm@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
--INSERT INTO USER_INFO VALUES (N'myduyen', N'e10adc3949ba59abbe56e057f20f883e', N'Duyen', N'Phieu', N'myduyen@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
--INSERT INTO USER_INFO VALUES (N'nghiaNgu', N'e10adc3949ba59abbe56e057f20f883e', N'nguvl', N'Phieu', N'nghiangu@gmail.com', 1, N'''photoURL''', 1,'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person1', N'e10adc3949ba59abbe56e057f20f883e', N'P1', N'Phieu', N'person1@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person2', N'e10adc3949ba59abbe56e057f20f883e', N'P2', N'Phieu', N'person2@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person3', N'e10adc3949ba59abbe56e057f20f883e', N'P3', N'Phieu', N'person3@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person4', N'e10adc3949ba59abbe56e057f20f883e', N'P4', N'Phieu', N'person4@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person5', N'e10adc3949ba59abbe56e057f20f883e', N'P5', N'Phieu', N'person5@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person6', N'e10adc3949ba59abbe56e057f20f883e', N'P6', N'Phieu', N'person6@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person7', N'e10adc3949ba59abbe56e057f20f883e', N'P7', N'Phieu', N'person7@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person8', N'e10adc3949ba59abbe56e057f20f883e', N'P8', N'Phieu', N'person8@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person9', N'e10adc3949ba59abbe56e057f20f883e', N'P9', N'Phieu', N'person9@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person10', N'e10adc3949ba59abbe56e057f20f883e', N'P10', N'Phieu', N'person10@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person11', N'e10adc3949ba59abbe56e057f20f883e', N'P11', N'Phieu', N'person11@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person12', N'e10adc3949ba59abbe56e057f20f883e', N'P12', N'Phieu', N'person12@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person13', N'e10adc3949ba59abbe56e057f20f883e', N'P13', N'Phieu', N'person13@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person14', N'e10adc3949ba59abbe56e057f20f883e', N'P14', N'Phieu', N'person14@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person15', N'e10adc3949ba59abbe56e057f20f883e', N'P15', N'Phieu', N'person15@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person16', N'e10adc3949ba59abbe56e057f20f883e', N'P16', N'Phieu', N'person16@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person17', N'e10adc3949ba59abbe56e057f20f883e', N'P17', N'Phieu', N'person17@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person18', N'e10adc3949ba59abbe56e057f20f883e', N'P18', N'Phieu', N'person18@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person19', N'e10adc3949ba59abbe56e057f20f883e', N'P19', N'Phieu', N'person19@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())
INSERT INTO USER_INFO VALUES (N'person20', N'e10adc3949ba59abbe56e057f20f883e', N'P20', N'Phieu', N'person20@gmail.com', 1, N'''photoURL''', 1, 'Vietnam', 'Description About', 1, 2019, 'facebookLink', 'googleLink',GETDATE())

--LANGUAGE
INSERT INTO LANGUAGE VALUES (N'Cpp', N'abc')
INSERT INTO LANGUAGE VALUES (N'CSharp', N'Create data migrate')
INSERT INTO LANGUAGE VALUES (N'Java', N'Create data migrate')

--COMPETE
INSERT INTO COMPETE VALUES(1,N'Project code challenge 1', 'project-code-challenge-1', N'Chỉ là mô tả thôi 1', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 2', 'project-code-challenge-2', N'Chỉ là mô tả thôi 2', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 3', 'project-code-challenge-3', N'Chỉ là mô tả thôi 3', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 4', 'project-code-challenge-4', N'Chỉ là mô tả thôi 4', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 5', 'project-code-challenge-5', N'Chỉ là mô tả thôi 5', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 6', 'project-code-challenge-6', N'Chỉ là mô tả thôi 6', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 7', 'project-code-challenge-7', N'Chỉ là mô tả thôi 7', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 8', 'project-code-challenge-8', N'Chỉ là mô tả thôi 8', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 9', 'project-code-challenge-9', N'Chỉ là mô tả thôi 9', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 10', 'project-code-challenge-10', N'Chỉ là mô tả thôi 10', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 11', 'project-code-challenge-11', N'Chỉ là mô tả thôi 11', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 12', 'project-code-challenge-12', N'Chỉ là mô tả thôi 12', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 13', 'project-code-challenge-13', N'Chỉ là mô tả thôi 13', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 14', 'project-code-challenge-14', N'Chỉ là mô tả thôi 14', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 15', 'project-code-challenge-15', N'Chỉ là mô tả thôi 15', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 16', 'project-code-challenge-16', N'Chỉ là mô tả thôi 16', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 17', 'project-code-challenge-17', N'Chỉ là mô tả thôi 17', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 18', 'project-code-challenge-18', N'Chỉ là mô tả thôi 18', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 19', 'project-code-challenge-19', N'Chỉ là mô tả thôi 19', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)
INSERT INTO COMPETE VALUES(1,N'Project code challenge 20', 'project-code-challenge-20', N'Chỉ là mô tả thôi 20', N'Đây là phần rule của compete', 10000, GETDATE(), 10000)


--CHALLENGE
INSERT INTO CHALLENGE VALUES (1, N'test number 1', N'test-number-1', N'test 1', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 2', N'test-number-2', N'test 2', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 3', N'test-number-3', N'test 3', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 4', N'test-number-4', N'test 4', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 5', N'test-number-5', N'test 5', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 6', N'test-number-6', N'test 6', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 7', N'test-number-7', N'test 7', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 8', N'test-number-8', N'test 8', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 9', N'test-number-9', N'test 9', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 10', N'test-number-10', N'test 10', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 11', N'test-number-11', N'test 11', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 12', N'test-number-12', N'test 12', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 13', N'test-number-13', N'test 13', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 14', N'test-number-14', N'test 14', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 15', N'test-number-15', N'test 15', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 16', N'test-number-16', N'test 16', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 17', N'test-number-17', N'test 17', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 18', N'test-number-18', N'test 18', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 19', N'test-number-19', N'test 19', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 20', N'test-number-20', N'test 20', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Cpp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 21', N'test-number-21', N'test 21', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 22', N'test-number-22', N'test 22', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 23', N'test-number-23', N'test 23', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 24', N'test-number-24', N'test 24', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 25', N'test-number-25', N'test 25', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 26', N'test-number-26', N'test 26', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 27', N'test-number-27', N'test 27', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 28', N'test-number-28', N'test 28', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 29', N'test-number-29', N'test 29', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 30', N'test-number-30', N'test 30', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 31', N'test-number-31', N'test 31', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 32', N'test-number-32', N'test 32', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 33', N'test-number-33', N'test 33', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 34', N'test-number-34', N'test 34', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 35', N'test-number-35', N'test 35', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 36', N'test-number-36', N'test 36', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 37', N'test-number-37', N'test 37', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 38', N'test-number-38', N'test 38', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 39', N'test-number-39', N'test 39', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 40', N'test-number-40', N'test 40', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'CSharp', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 41', N'test-number-41', N'test 41', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 42', N'test-number-42', N'test 42', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 43', N'test-number-43', N'test 43', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 44', N'test-number-44', N'test 44', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 45', N'test-number-45', N'test 45', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 46', N'test-number-46', N'test 46', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 47', N'test-number-47', N'test 47', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 48', N'test-number-48', N'test 48', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 49', N'test-number-49', N'test 49', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 50', N'test-number-50', N'test 50', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 51', N'test-number-51', N'test 51', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 52', N'test-number-52', N'test 52', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 53', N'test-number-53', N'test 53', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 54', N'test-number-54', N'test 54', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 55', N'test-number-55', N'test 55', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 56', N'test-number-56', N'test 56', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 57', N'test-number-57', N'test 57', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 58', N'test-number-58', N'test 58', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 59', N'test-number-59', N'test 59', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)
INSERT INTO CHALLENGE VALUES (1, N'test number 60', N'test-number-60', N'test 60', N'problem statement', N'abcd', N'efgh', 1, N'1', 60, 100, N'gdfgdfg', N'Java', 1, 1, 1, 0, 0, 0, 0, 0, null, null, null, 0, null , null, null, null, null)

--ANSWER
INSERT INTO ANSWER VALUES (1, 1, N'sgsdgf', 1, GETDATE())
INSERT INTO ANSWER VALUES (21, 1, N'rtrte', 1, GETDATE())
INSERT INTO ANSWER VALUES (41, 1, N'rtrte', 1, GETDATE())

--CHANLLENGE_COMPETE
INSERT INTO CHALLENGE_COMPETE VALUES (1, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (2, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (3, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (4, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (5, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (6, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (7, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (8, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (9, 1)
INSERT INTO CHALLENGE_COMPETE VALUES (10, 1)

--CHALLENGE_LANGUAGE
INSERT INTO CHALLENGE_LANGUAGE VALUES (1, 1, N'code stub cpp')
INSERT INTO CHALLENGE_LANGUAGE VALUES (1, 2, N'code stub csharp')
INSERT INTO CHALLENGE_LANGUAGE VALUES (1, 3, N'code stub java')
INSERT INTO CHALLENGE_LANGUAGE VALUES (21, 1, N'code stub cpp')
INSERT INTO CHALLENGE_LANGUAGE VALUES (21, 2, N'code stub csharp')
INSERT INTO CHALLENGE_LANGUAGE VALUES (21, 3, N'code stub java')
INSERT INTO CHALLENGE_LANGUAGE VALUES (41, 1, N'code stub cpp')
INSERT INTO CHALLENGE_LANGUAGE VALUES (41, 2, N'code stub csharp')
INSERT INTO CHALLENGE_LANGUAGE VALUES (41, 3, N'code stub java')

--CHALLENGE_EDITOR
INSERT INTO CHALLENGE_EDITOR VALUES (1, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (2, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (3, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (4, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (5, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (6, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (7, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (8, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (9, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (10, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (11, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (12, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (13, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (14, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (15, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (16, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (17, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (18, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (19, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (20, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (21, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (22, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (23, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (24, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (25, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (26, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (27, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (28, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (29, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (30, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (31, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (32, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (33, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (34, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (35, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (36, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (37, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (38, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (39, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (40, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (41, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (42, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (43, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (44, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (45, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (46, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (47, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (48, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (49, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (50, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (51, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (52, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (53, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (54, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (55, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (56, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (57, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (58, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (59, 1)
INSERT INTO CHALLENGE_EDITOR VALUES (60, 1)


--TEST_CASE
INSERT INTO TESTCASE VALUES (41, 'challenge_41_input_0.txt', 'challenge_41_output_0.txt')
INSERT INTO TESTCASE VALUES (41, 'challenge_41_input_1.txt', 'challenge_41_output_1.txt')
INSERT INTO TESTCASE VALUES (41, 'challenge_41_input_2.txt', 'challenge_41_output_2.txt')
INSERT INTO TESTCASE VALUES (42, 'challenge_42_input_0.txt', 'challenge_42_output_0.txt')
INSERT INTO TESTCASE VALUES (42, 'challenge_42_input_1.txt', 'challenge_42_output_1.txt')
INSERT INTO TESTCASE VALUES (42, 'challenge_42_input_2.txt', 'challenge_42_output_2.txt')

--COMMENT
INSERT INTO COMMENT ([Text], [CreateDate], [Likes], [OwnerID], [ChallengeID]) VALUES (N'Comment choi thoi', CAST(N'2019-01-01 00:00:00.000' AS DateTime), 1, 2, 2)

--REPLY
INSERT INTO REPLY ([CommentID], [OwnerID], [Text], [CreateDate], [Likes]) VALUES (1, 2, N'Reply choi thoi', CAST(N'2019-01-10 00:00:00.000' AS DateTime), 10)