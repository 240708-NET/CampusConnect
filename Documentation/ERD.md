# Blog DB ERD

## User:
- ID (int, not null)
- username (varchar, not null)
- password (varchar, not null)
- admin (bool)

## Category:
- ID (int, not null)
- Name (varchar, not null)

## Post:
- ID (int not null)
- CategoryID (int, foreign key)
- UserID (int, foreign key)
- Topic (varchar, not null)
- Body (varchar, not null)
- CreateAt (DateTime)
- EditedAt (DateTime)

## Comment:
- ID (int, not null, foreign key)
- PostID (int, not null, foreign key)
- UserID (int, not null, foreign key)
- ReplyID (int, foreign key)
- Body (varchar, not null)
- CreateAt (DateTime)
- EditedAt (DateTime)