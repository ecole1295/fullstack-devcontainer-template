db = db.getSiblingDB('devdb');

db.createUser({
  user: 'devuser',
  pwd: 'devpassword',
  roles: [
    {
      role: 'readWrite',
      db: 'devdb'
    }
  ]
});

db.createCollection('samples');
db.samples.insertOne({
  message: 'MongoDB is ready for development!',
  createdAt: new Date()
});

print('Development database initialized successfully');