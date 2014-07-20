-- open/connect you mysql database and simple run the below.
-- thats it, you are set for user registration and login

grant all privileges on 
	thePortal.* to 
	'dev'@'%' identified by 
	'dev@theportal' with grant option;

flush privileges;

-- --test
use mysql;
select host, user from user where user = 'dev';
select host, user, db from db where user = 'dev';
