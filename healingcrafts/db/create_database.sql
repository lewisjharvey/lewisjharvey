DROP DATABASE IF EXISTS healingcrafts_development;
DROP DATABASE IF EXISTS healingcrafts_test;
DROP DATABASE IF EXISTS healingcrafts_production;

CREATE DATABASE healingcrafts_development;
CREATE DATABASE healingcrafts_test;
CREATE DATABASE healingcrafts_production;

GRANT ALL ON healingcrafts_development.* TO 'lewisharvey'@'localhost' IDENTIFIED BY 'Fitzroyrobin';
GRANT ALL ON healingcrafts_test.* TO 'lewisharvey'@'localhost' IDENTIFIED BY 'Fitzroyrobin';
GRANT ALL ON healingcrafts_production.* TO 'lewisharvey'@'localhost' IDENTIFIED BY 'Fitzroyrobin';
