drop table Items
CREATE TABLE Items(
	[Id] int IDENTITY(1,1) PRIMARY KEY,
	[Name] nvarchar(127),
	[Description] nvarchar(max),
	[FilePath] nvarchar(max)
)


INSERT INTO Items VALUES('Amazon', 'Amazon Cough Syrup', 'Amazon.jpg'),
('Chestal','Chestal Cough Syrup', 'Chestal.jpg'),
('Gaia','Gaia Cough Syrup', 'Gaia.webp'),
('Matys', 'Matys Natural Cough Syrup', 'Matys.webp'),
('Rite Aid', 'Rite Aid Cough Syrup', 'RiteAid.webp'),
('Robitussin','Robitussin Cough Syrup', 'Robitussin.jpg'),
('Wellness', 'Wellness Cough Syrup', 'Wellness.PNG')

select * from Items