SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
TRUNCATE `tentrydetail`;
TRUNCATE `tentry`;
TRUNCATE `tsalesinvoice`;
TRUNCATE `tsalesinvoicedetail`;
TRUNCATE `tinventory`;
TRUNCATE `tinventorycost`;
TRUNCATE `tquantitycreditinginvoice`;
TRUNCATE `tquantitycreditinginvoicedetail`;
TRUNCATE `tpurchaseinvoice`;
TRUNCATE `tpurchaseinvoicedetail`;
TRUNCATE `tdiscountiteditem`;
TRUNCATE `tsellreturninvoice`;
TRUNCATE `tsellreturninvoicedetail`;
TRUNCATE `tmarketerdisbursementpermit`;
TRUNCATE `tmarketerdisbursementpermitdetail`;
TRUNCATE `tmarketerinventory`;
SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1);

set @maxtimetime = 10;
-- 1
DELETE FROM tentrydetail WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
DELETE FROM tentry WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
-- 2
DELETE FROM tmarketerinventory WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
DELETE FROM tinventorycost WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
DELETE FROM tinventory WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
-- 3
DELETE FROM tpurchaseinvoicedetail WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
DELETE FROM tpurchaseinvoice WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
-- 4
DELETE FROM tquantitycreditinginvoicedetail WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
DELETE FROM tquantitycreditinginvoice WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
-- 5
DELETE FROM tsalesinvoicedetail WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
DELETE FROM tsalesinvoice WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
-- 6
DELETE FROM tsellreturninvoicedetail WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
DELETE FROM tsellreturninvoice WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
-- 7
DELETE FROM tmarketerdisbursementpermitdetail WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
DELETE FROM tmarketerdisbursementpermit WHERE isdeleted=1 AND DATEDIFF(CURDATE(), CreateDate) > @maxtime;
-- x