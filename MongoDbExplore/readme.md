# Exploring MongoDB with C#

Exploring Different features of MongoDB using C#

- `MongoDbSampleDataGenerator` contains code which generates sample data and puts them to database.
- `MongoDbChangeStreamProcessor` contains code exploring features of `MongoDB Change Stream`
- `mongodb-recplica-set-with-initsh-file.yml` together with `init.sh` contains the docker compose to run local mongodb cluster for exploration. **`THIS IS WORKING`**
- `mogodb-replica-set.yml` has the `init.sh` content in the command string itself. But **`THIS DOES NOT WORK`** sometimes and needs to run this `mongoinit` container again after all 3 container for cluster are up and running, for it to work. **`Need to explore this more`**

## TODO

- Adding Authentication on MongoDB Cluster
- Fixing issue in `mogodb-replica-set.yml`
