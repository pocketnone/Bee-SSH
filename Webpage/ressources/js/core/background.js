(function() {
    var Particle, particleCount, particles, sketch, z;

    sketch = Sketch.create();

    particles = [];

    particleCount = 150;

    sketch.mouse.x = sketch.width / 2;

    sketch.mouse.y = sketch.height / 2;

    sketch.strokeStyle = 'hsla(200, 50%, 50%, .4)';

    sketch.globalCompositeOperation = 'lighter';

    Particle = function() {
        this.x = random(sketch.width);
        this.y = random(sketch.height, sketch.height * 2);
        this.vx = 0;
        this.vy = -random(1, 10) / 5;
        this.radius = this.baseRadius = 1;
        this.maxRadius = 150;
        this.threshold = 150;
        return this.hue = random(180, 240);
    };

    Particle.prototype = {
        update: function() {
            var dist, distx, disty;
            distx = this.x - sketch.mouse.x;
            disty = this.y - sketch.mouse.y;
            dist = sqrt(distx * distx + disty * disty);
            this.vx += (random(50) - 25) / 1000;
            this.vy -= random(1, 10) / 999999;
            this.x += this.vx;
            this.y += this.vy;
            if (this.x < -this.maxRadius || this.x > sketch.width + this.maxRadius || this.y < -this.maxRadius) {
                this.x = random(sketch.width);
                this.y = random(sketch.height + this.maxRadius, sketch.height * 2);
                this.vx = 0;
                return this.vy = -random(1, 10) / 5;
            }
        },
        render: function() {
            sketch.beginPath();
            sketch.arc(this.x, this.y, this.radius, 0, TWO_PI);
            sketch.closePath();
            sketch.fillStyle = 'hsla(' + this.hue + ', 60%, 40%, .35)';
            sketch.fill();
            return sketch.stroke();
        }
    };

    z = particleCount;

    while (z--) {
        particles.push(new Particle());
    }

    sketch.clear = function() {
        return sketch.clearRect(0, 0, sketch.width, sketch.height);
    };

    sketch.update = function() {
        var i, _results;
        i = particles.length;
        _results = [];
        while (i--) {
            _results.push(particles[i].update());
        }
        return _results;
    };

    sketch.draw = function() {
        var i, _results;
        i = particles.length;
        _results = [];
        while (i--) {
            _results.push(particles[i].render());
        }
        return _results;
    };

}).call(this);