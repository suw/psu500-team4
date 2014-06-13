# Processor 

app(stg):54.213.25.60:1337

app(prd):54.187.176.214:1337

jenkins: 54.187.21.48:8080

sonar:   54.186.5.185:9000:9000	admin/admin

mongo:	 54.187.141.162

		 54.187.31.228

		 54.200.152.131

	     54.187.141.162

## How to start app

**Start Mongo**

`$ mongod --smallfiles`

**Start SailsJS**

`$ cd processor`

`$ sails lift`

## How to run Karma tests

**Start the app (See above to start the app)**

**Start the test runner**

`$ cd processor`

`$ ./node_modules/karma/bin/karma start`
