setting postgres local

Step 2: Create Role With Password:

The next step is to create a role with password, for this we will have to follow the following syntax:

CREATE ROLE role_name WITH LOGIN PASSWORD password_for_user;
CREATE ROLE uploadfileadmin WITH LOGIN PASSWORD 'admin12345';

fileupload

migrations: 

dotnet ef migrations add InitialCreate   
init on migations on my data like files

dotnet ef database update


