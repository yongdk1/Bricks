README for Bricks

Prerequisites
1. Visual Studio 2019 (With ASP.NET and web development installed)
2. MySQL Workbench 8.0 CE

Visual Studio Setup:

Installing Packages in Visual Studio 2019
1. Open the project in Visual Studio 2019
2. Open the "Solution Explorer"
3. Expand "Dependencies"
4. Right click "Packages" and select "Manage NuGet Packages"
5. Install the following packages in Visual Studio 2019:
   - Microsoft.EntityFrameworkCore.SqlServer v3.1.8
   - Microsoft.EntityFrameworkCore.Tools v3.1.8
   - Microsoft.VisualStudio.Web.CodeGeneration.Des v3.1.4
   - Newtonsoft.Json v12.0.3
   - Pomelo.EntityFrameworkCore.MySql v3.2.3
   - System.Net.Http.Json v3.2.1

Database:

Database Setup
1. Open MySQL Workbench 8.0 CE
2. Select a MySQL Connection
3. Create a schema call bricks
4. Expand the schema and right click on Tables and select "Create Table"
5. Name the table as pmiresidenceresult and click Apply
6. Copy and paste the following below in the SQL Script box and click Apply
   CREATE TABLE `bricks`.`pmiresidenceresult` (
  `idtransaction` int NOT NULL AUTO_INCREMENT,
  `street` varchar(500) DEFAULT NULL,
  `x` varchar(45) DEFAULT NULL,
  `project` varchar(45) DEFAULT NULL,
  `y` varchar(45) DEFAULT NULL,
  `area` varchar(45) DEFAULT NULL,
  `floorRange` varchar(45) DEFAULT NULL,
  `noOfUnits` varchar(45) DEFAULT NULL,
  `contractDate` varchar(45) DEFAULT NULL,
  `typeOfSale` varchar(45) DEFAULT NULL,
  `price` varchar(45) DEFAULT NULL,
  `propertyType` varchar(45) DEFAULT NULL,
  `district` varchar(45) DEFAULT NULL,
  `typeOfArea` varchar(45) DEFAULT NULL,
  `tenure` varchar(45) DEFAULT NULL,
  `marketSegment` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idtransaction`)
  );
7. Check in the Navigator that the table has been created

Changing Database Connection to Your Local DB
1. Go to "appsettings.json" and change line 11 to your local DB username and password
2. Go to "PullData.cs" and change line 18 to your local DB username and password
3. Go to TimedHostedServices and change line 93 to your local DB username and password

Database and Ura API Information
We call the Ura API to send data to our database. The Ura API token refreshes every 24 hours. 
To change the token:
1. Go to "UraService.cs" under Services file
2. Update the token in line 24 to a new token
3. If you require a new token, please contact 2006bricks@gmail.com
4. Run the web application
5. Verify that the data is stored in MySQL Workbench
6. You can verify by running the following query in MySQL
   select * from bricks.pmiresidenceresult
7. If you are unable to get the any data in database due to a expiration of token of incorrect setup of DB, please proceed to the section "Database using dumps"

Database using dumps
1. Connect to your MySQL database
2. Click "Server" on the tool bar
3. Select "Data Import"
4. Navigate the link to where the dumps file (bricks_pmiresidenceresult.sql) is located
5. Select the bricks schema to restore
6. Select "Start Import" on the bottom right