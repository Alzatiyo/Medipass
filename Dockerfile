// Inicialización de la base de datos EHR Logger
db = db.getSiblingDB('ehr_db');

db.createUser({
    user: 'ehrlogger_user',
    pwd:  'ehrlogger_pass',
    roles: [{ role: 'readWrite', db: 'ehr_db' }]
});

db.createCollection('ehr_records', {
    validator: {
        $jsonSchema: {
            bsonType: 'object',
            required: ['appointmentId', 'patientId', 'doctorId', 'specialty', 'status'],
            properties: {
                appointmentId: { bsonType: 'string' },
                patientId:     { bsonType: 'string' },
                status:        { bsonType: 'string',
                    enum: ['Pending', 'Stored', 'Failed', 'DeadLetter'] }
            }
        }
    }
});

db.ehr_records.createIndex({ appointmentId: 1 }, { unique: true });
db.ehr_records.createIndex({ patientId: 1 });
db.ehr_records.createIndex({ status: 1 });
db.ehr_records.createIndex({ correlationId: 1 });
db.ehr_records.createIndex({ occurredAt: -1 });

print('MS-EHRLogger (.NET): base de datos inicializada');
