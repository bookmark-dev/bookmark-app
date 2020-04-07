const express = require('express');
const path = require('path');
const bcrypt = require('bcryptjs');
const cors = require('cors');

const server = express();
server.use(express.urlencoded({ extended: true }));
server.use(express.static(path.join(__dirname, '/public/dist/public')));
server.use(express.json());
server.use(cors());

/*server.post('/api/hash', (req, res) => {
    let password = req.body['password'];
    bcrypt.hash(password, bcrypt.genSalt(10))
    .then(result => res.json(result))
    .catch(err => res.json(err));
});*/

server.post('/api/compare', (req, res) => {
    let a = req.body['password'];
    let b = req.body['hashed'];
    bcrypt.compare(a, b)
    .then(result => res.json(result))
    .catch(err => res.json(err));
});

server.get('*', (_req, res) => {
    res.sendFile(path.resolve('./public/dist/public/index.html'));
});

server.listen(8000, console.log('Listening at port 8000!'));
