# Be sure to restart your server when you modify this file.

# Your secret key for verifying cookie session data integrity.
# If you change this key, all old sessions will become invalid!
# Make sure the secret is at least 30 characters and all random, 
# no regular words or you'll be exposed to dictionary attacks.
ActionController::Base.session = {
  :key    => '_realalemap_session',
  :secret => 'b40fccaa038553bedf1b31a1abc80c1ea99398c32f36f5fe50eaf81e00dd7e1ed7f40287a8aa6593944029b08e5583b05ab28c58a47997e39b84ae422fe70677'
}

# Use the database for sessions instead of the cookie-based default,
# which shouldn't be used to store highly confidential information
# (create the session table with "rake db:sessions:create")
# ActionController::Base.session_store = :active_record_store
