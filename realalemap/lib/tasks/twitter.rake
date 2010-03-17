require 'twitter'

namespace :twitter do
	desc "Polls the twitter account for new drink entries"
	task :poll => [:environment] do
	
		# If the seed doesn't exist, get the last post to stat from
		tmp_file = File.join("#{RAILS_ROOT}/tmp", 'twitter_seed')
		if File.file?(tmp_file)
			puts "File Exists: " + tmp_file
			last_id = File.read(tmp_file).to_i    
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
			puts "Added: " + status.text + "ID:" + status.id.to_s
			last_id = (status.id.to_i > last_id.to_i ? status.id.to_i : last_id.to_i)
		}
	
		# Delete the contents of the file
		if timeline.results.count > 0
			f = File.open(tmp_file, 'w+')
			f.truncate(0)
			f.write(last_id.to_s)
			f.close		
			puts "LastId To File: " + last_id.to_s
		end
	end
	
	desc "Processes drink reports from the database"
	task :process => [:environment] do
		# No need to check the process is not running, cron will do this.
		@drink_reports = DrinkReport.find(:all)
    @drink_reports.each { |d|
      puts d.text
    }		
	end
end
