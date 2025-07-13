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

db.createCollection('Settings');
db.Settings.insertMany([
  {
    key: "app.name",
    value: "Fullstack Application",
    description: "The name of the application",
    category: "General",
    isEncrypted: false,
    createdAt: new Date(),
    updatedAt: new Date()
  },
  {
    key: "app.version",
    value: "1.0.0",
    description: "Current version of the application",
    category: "General",
    isEncrypted: false,
    createdAt: new Date(),
    updatedAt: new Date()
  },
  {
    key: "app.theme.primary_color",
    value: "#1976d2",
    description: "Primary color for the application theme",
    category: "Theme",
    isEncrypted: false,
    createdAt: new Date(),
    updatedAt: new Date()
  },
  {
    key: "app.maintenance.enabled",
    value: "false",
    description: "Whether maintenance mode is enabled",
    category: "Maintenance",
    isEncrypted: false,
    createdAt: new Date(),
    updatedAt: new Date()
  },
  {
    key: "app.api.timeout",
    value: "30000",
    description: "API timeout in milliseconds",
    category: "API",
    isEncrypted: false,
    createdAt: new Date(),
    updatedAt: new Date()
  }
]);

db.Settings.createIndex({ "key": 1 }, { unique: true });
db.Settings.createIndex({ "category": 1 });
db.Settings.createIndex({ "createdAt": 1 });

print('Development database initialized successfully');
print("Inserted " + db.Settings.countDocuments() + " initial settings.");