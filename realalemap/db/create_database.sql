DROP DATABASE IF EXISTS realalemap_development;
DROP DATABASE IF EXISTS realalemap_test;
DROP DATABASE IF EXISTS realalemap_production;

CREATE DATABASE realalemap_development;
CREATE DATABASE realalemap_test;
CREATE DATABASE realalemap_production;

GRANT ALL ON realalemap_development.* TO 'realalemap'@'localhost' IDENTIFIED BY '';
GRANT ALL ON realalemap_test.* TO 'realalemap'@'localhost' IDENTIFIED BY '';
GRANT ALL ON realalemap_production.* TO 'realalemap'@'localhost' IDENTIFIED BY '';
