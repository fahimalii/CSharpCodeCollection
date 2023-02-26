#!/bin/sh
echo 'Sleeping for 30 seconds, to ensure mongo1, mongo2, mongo3 are up and running.' &&
echo 'Modify sleep time if needed' &&
sleep 30 &&
mongosh --host mongo1:27017 --eval 'rs.initiate({
    "_id" : "rs0",
    "members" :
    [
        {
            "_id" : 0,
            "host" : "<YOUR_HOST_MACHINE_IP_ADDRESS>:27021"
        },
        {
            "_id" : 1,
            "host" : "<YOUR_HOST_MACHINE_IP_ADDRESS>:27022"
        },
        {
            "_id" : 2,
            "host" : "<YOUR_HOST_MACHINE_IP_ADDRESS>:27023"
        }
    ]
})';