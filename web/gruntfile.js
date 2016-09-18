module.exports = function(grunt) {

    // 1. Вся настройка находится здесь
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        concat: {
            dist: {
                src: [
                    'bower_components/bootstrap/js/*.js', // Все JS в папке libs

                ],
                dest: 'build/global.js',
            }
        },

        uglify: {
            build: {
                src: 'build/global.js',
                dest: 'build/global.min.js',
            }
        },

        wiredep: {

            task: {

                // Point to the files that should be updated when
                // you run `grunt wiredep`
                src: [
                    'index.html'                                    
                ],

                options: {
                    // See wiredep's configuration documentation for the options
                    // you may pass:

                    // https://github.com/taptapship/wiredep#configuration
                }
            }
        }

    });

    // 3. Тут мы указываем Grunt, что хотим использовать этот плагин
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-wiredep');

    // 4. Указываем, какие задачи выполняются, когда мы вводим «grunt» в терминале
    grunt.registerTask('default', ['concat', 'uglify']);


};
