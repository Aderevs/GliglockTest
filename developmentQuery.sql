SELECT * FROM Tests;
SELECT * FROM Questions;
SELECT * FROM AnswerOptions;
SELECT * FROM Students;
SELECT * FROM Teachers;
SELECT * FROM PassedTests;

INSERT INTO Teachers
(id, PasswordHash, Salt, FirstName, LastName, Email)
VALUES
('00000000-0000-0000-0000-000000000000', 'q][wepmdsvmg87zsjvknk654f9d6v5fv', NEWID(), 'Teacher', 'First', 'firstTeach@ukr.net');

INSERT INTO Tests
VALUES
('00000000-0000-0000-0000-000000000000', 'first_test', 10, '00000000-0000-0000-0000-000000000000');


INSERT INTO Questions
VALUES
('00000000-0000-0000-0000-000000000001', '������� ��������� �������:', '00000000-0000-0000-0000-000000000000');


INSERT INTO AnswerOptions
VALUES
(NEWID(), '�����1', 0, '00000000-0000-0000-0000-000000000001'),
(NEWID(), '���������1', 1, '00000000-0000-0000-0000-000000000001'),
(NEWID(), '�����1', 0, '00000000-0000-0000-0000-000000000001'),
(NEWID(), '�����1', 0, '00000000-0000-0000-0000-000000000001');






INSERT INTO Questions
VALUES
('00000000-0000-0000-0000-000000000002', '������� ����� ���������� ��������:', '00000000-0000-0000-0000-000000000000');

INSERT INTO AnswerOptions
VALUES
(NEWID(), '�����2', 0, '00000000-0000-0000-0000-000000000002'),
(NEWID(), '���������2', 1, '00000000-0000-0000-0000-000000000002'),
(NEWID(), '�����2', 0, '00000000-0000-0000-0000-000000000002'),
(NEWID(), '���������2', 1, '00000000-0000-0000-0000-000000000002');



UPDATE Questions
SET
WithImg = 0
WHERE Id='00000000-0000-0000-0000-000000000001'

DELETE
FROM Tests
WHERE Name IN ('test with image', 'test test')