const express = require('express');
const app = express();

app.use('/', express.static('frontend/build'));

app.get('/api', (req, res) => {
  res.send('Hello, world!');
});

app.listen(3000, () => {
  console.log('Server listening on port 3000');
});
