-- open/connect to your mysql database and simple run the below scripts
-- thats it, you are set for user registration and login
-- everything is taken care of

grant all privileges on 
	thePortal.* to 
	'dev'@'%' identified by 
	'dev@theportal' with grant option;

flush privileges;

-- --test
use mysql;
select host, user from user where user = 'dev';
select host, user, db from db where user = 'dev';
