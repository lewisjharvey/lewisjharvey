require 'twitter'

namespace :twitter do
	desc "Polls the twitter account for new drink entries"
	task :poll => [:environment] do
		# Setup the twitter account
		#tweet = YAML::load(File.open('twitter.yml'))
		#@twitter = Twitter::Base.new(tweet['username'], tweet['password'])
	
		# If the seed doesn't exist, get the last post to stat from
		tmp_file = File.join("#{RAILS_ROOT}/tmp", 'twitter_seed')
		if File.file?(tmp_file)
			puts "File Exists: " + tmp_file
			last_id = File.read(tmp_file)    
		else
			last_id = 0
		end
		puts "Last Id: " + last_id.to_s
	
		timeline = Twitter::Search.new.to('realalemap').since(last_id).fetch()
		puts "Results: " + timeline.results.count.to_s
		timeline.results.each { |status|
			d = DrinkReport.new
			d.from_user = status.from_user
			d.text = status.text
			d.twitter_id = status.id
			d.save
			puts "Added: " + status.text
		}
	
		# Delete the contents of the file
		if timeline.results.count > 0
			f = File.open(tmp_file, 'w+')
			f.truncate(0)
			f.write(timeline.results.last.id)
			f.close		
			puts "LastId To File: " + timeline.results.last.id.to_s
		end
	end
end
