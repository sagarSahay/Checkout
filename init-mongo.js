let error = true

let res = [
  db.container.drop(),
  db.container.createIndex({ myfield: 1 }, { unique: true }),
  db.container.createIndex({ thatfield: 1 }),
  db.container.createIndex({ thatfield: 1 }),
  db.container.insert({ myfield: 'hello', thatfield: 'testing' }),
  db.other.
]

printjson(res)

if (error) {
  print('Error, exiting')
  quit(1)
}