class CreateDrinkReports < ActiveRecord::Migration
  def self.up
    create_table :drink_reports do |t|
      t.string "text", :null => false
      t.string "twitter_id", :null => false
      t.string "from_user", :null => false
      t.timestamps
    end
  end

  def self.down
    drop_table :drink_reports
  end
end
        
