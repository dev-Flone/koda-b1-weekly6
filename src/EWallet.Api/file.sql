-- SELECT cc.name AS ConstraintName,
--        cc.definition AS ConstraintDefinition
-- FROM   sys.check_constraints AS cc
--        INNER JOIN
--        sys.tables AS t
--        ON cc.parent_object_id = t.object_id
-- WHERE  t.name = 'Wallets';
-- ALTER TABLE Wallets DROP CONSTRAINT Ck_Wallets_Balance;
-- ALTER TABLE Wallets
--        ADD CONSTRAINT Ck_Wallets_Balance CHECK (Balance >= 0);
-- SELECT COLUMN_NAME,
--        IS_NULLABLE,
--        DATA_TYPE
-- FROM   INFORMATION_SCHEMA.COLUMNS
-- WHERE  TABLE_NAME = 'Transactions'
--        AND COLUMN_NAME = 'Description';
-- ALTER TABLE Transactions ALTER COLUMN Description NVARCHAR (250) NULL;
-- -- Standard ANSI Query
-- SELECT TABLE_NAME,
--        TABLE_TYPE
-- FROM   INFORMATION_SCHEMA.TABLES;
-- -- T-SQL System Views Query
SELECT COUNT(*) AS TotalUsers
FROM   dbo.Users;

SELECT COUNT(*) AS TotalWallets
FROM   dbo.Wallets;

SELECT name AS ObjectName,
       type_desc AS ObjectType
FROM   sys.objects
WHERE  type IN ('U', 'V'); -- 'U' = User Table, 'V' = View

SELECT table_name
FROM   information_schema.tables
WHERE  table_type = 'BASE TABLE';

DBCC CHECKIDENT ('Users', RESEED, 0);

DELETE Users;

DBCC CHECKIDENT ('Wallets', RESEED, 0);

DELETE Wallets;

SELECT *
FROM   Users;

SELECT *
FROM   Wallets;

SELECT *
FROM   Transactions;

SELECT *
FROM   WalletMutations;

SELECT *
FROM   PaymentMethods;

SELECT *
FROM   TopUpRequests;

SELECT *
FROM   AuditLogs;

SELECT (SELECT COUNT(*)
        FROM   Transactions) AS TotalTransactions,
       (SELECT COUNT(*)
        FROM   AuditLogs) AS TotalAuditLogs;

SELECT ReferenceNo,
       Id,
       Status
FROM   Transactions
WHERE  ReferenceNo LIKE 'SEED-%';

-- Reset DATABASE
BEGIN TRANSACTION;

DELETE AuditLogs;

DELETE WalletMutations;

DELETE TopUpRequests;

DELETE Transactions;

DELETE Wallets;

DELETE PaymentMethods;

DELETE Users;

COMMIT TRANSACTION;

SELECT   Id,
         Code,
         Name,
         IsActive
FROM     PaymentMethods
ORDER BY Id;

SELECT   *
FROM     Transactions
ORDER BY Id DESC;

SELECT   *
FROM     TopUpRequests
ORDER BY Id DESC;