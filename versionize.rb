require 'fileutils'

# ask for version
puts "> enter version (format x.x.x)..."
version = STDIN.gets.chomp + '.0'
fail "error: '#{version}' is not in the format 'x.x.x'." unless version.match(/\d\.\d\.\d/)

# update version
['version.txt', 'Koffeinfrei.Zueribad/Properties/AssemblyInfo.cs'].each do |filename|
	puts "> updating '#{filename}'..."
	contents = File.read(filename)
	File.open(filename, 'w') do |file|
		file.puts contents.gsub(/\d\.\d\.\d\.\d/, "#{version}")
	end
end

# done
puts '> done.'