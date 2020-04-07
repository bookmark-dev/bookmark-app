const express = require('express');
const path = require('path');
const bcrypt = require('bcryptjs');

const server = express();
server.use(express.urlencoded({ extended: true }));
server.use(express.static(path.join(__dirname, '/public/dist/public')));
server.use(express.json());

server.get('*', (_req, res) => {
    res.sendFile(path.resolve('./public/dist/public/index.html'));
});

server.listen(8000, console.log('Listening at port 8000!'));
